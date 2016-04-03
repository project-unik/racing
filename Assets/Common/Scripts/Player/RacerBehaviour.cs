/*!
 * @file Assets/Behaviours/CameraBehaviour.cs
 * @created 2016/02/28
 * @lastmodified 2016/02/28
 * @brief Implements the camera behaviour following the player.
 *   Must be placed on the target camera object.
 *
 * Todo
 * ====
 *
 * - Make vehicle accelerate into the direction the camera is facing.
 */

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RacerBehaviour : NetworkBehaviour
{
    private Rigidbody rigidBody;
    private float accDeadZone = 0.1f;
    private float curThrust = 0.0f;
    private float curTurn = 0.0f;
    private Transform[] hoverPoints;
    private bool isGoingForward = true;

    public float hoverForce = 2f;
    public float hoverStability = 0.3f;
    public float hoverSpeed = 9.0f;
    public float hoverHeight = 2f;
    public float tipOverStability = 100.0f;

    public Vector3 airFriction = new Vector3(5f, 5f, 7f);

    public float maxSpeed = 60f;
    public float accForward = 1100;
    public float accBackward = 700f;
    public float brakeStrength = 50f;

    public float baseTurnRadius = 1.8f;
    public float angularDrag = 7.0f;
    public float angularDragOnTurn = 2.0f;
    public float rotationCorrectionStrength = 3.0f;
    [Range(0.0f, 0.1f)]
    public float speedSlowdownOnTurn = 0.01f;
    public float forwardTorqueStrength = 0.25f;

    public GameObject cameraPrefab;

    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        int i = 0;
        hoverPoints = new Transform[4];
        foreach (Transform child in transform)
        {
            hoverPoints[i++] = child;
        }
        rigidBody.angularDrag = angularDrag;
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        Camera.main.GetComponent<CameraBehaviour>().TrackedObject = gameObject;
    }

    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        // Handle player input for acceleration.
        float vertical = Input.GetAxis(Tags.Input.VERTICAL);
        if (vertical > accDeadZone)
        {
            //set thrust forward
            curThrust = vertical * accForward;
        }
        // do not apply backwards thrust when in air
        else if (vertical < -accDeadZone && Physics.Raycast(transform.position, -transform.up, 1.5f * hoverHeight))
        {
            //set thrust backward
            curThrust = vertical * accBackward;
        }
        else
        {
            //turn off thrust
            curThrust = 0;
        }

        float horizontal = Input.GetAxis(Tags.Input.HORIZONTAL);
        if (Mathf.Abs(horizontal) > accDeadZone)
        {
            //turn sideward
            curTurn = horizontal;
        }
        else
        {
            //do not turn sideward
            curTurn = 0;
        }

        // lower than 60 to account for drifting
        isGoingForward = Vector3.Angle(transform.forward, rigidBody.velocity) <= 60.0f;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }


        // going forward
        if (curThrust >= 0f && rigidBody.velocity.magnitude <= maxSpeed)
        {
            rigidBody.AddForce(transform.forward * curThrust * Time.deltaTime, ForceMode.Acceleration);
        }
        // braking
        else if (curThrust < 0f && isGoingForward)
        {
            rigidBody.AddForce(-transform.forward * brakeStrength * rigidBody.velocity.magnitude * Time.deltaTime, ForceMode.Acceleration);
        }
        // going backward
        else if (curThrust < 0f && !isGoingForward)
        {
            rigidBody.AddForce(transform.forward * curThrust * Time.deltaTime, ForceMode.Acceleration);
        }

        if (curTurn != 0)
        {
            // Reduce angular drag while turning
            rigidBody.angularDrag = angularDragOnTurn;

            // Apply centripetal force for turning
            // Turning radius of racer increases with velocity
            float radius = baseTurnRadius * rigidBody.velocity.magnitude;
            float centripetalForce = curTurn * Mathf.Pow(rigidBody.velocity.magnitude, 2.0f) / radius;
            rigidBody.AddForce(centripetalForce * transform.right, ForceMode.Acceleration);

            rigidBody.AddForce(-rigidBody.velocity * Mathf.Abs(centripetalForce) * speedSlowdownOnTurn, ForceMode.Acceleration);

            // Racer leans into the curve
            rigidBody.AddTorque(-transform.forward * centripetalForce * forwardTorqueStrength, ForceMode.Acceleration);
        }
        else
        {
            //slow down the sideward rotation
            rigidBody.angularDrag = angularDrag;
        }

        // Correct rotation of racer
        {
            float forward = isGoingForward ? 1.0f : -1.0f;
            Vector3 forwardVelocity = Vector3.ProjectOnPlane(rigidBody.velocity, transform.up);
            float rotationCorrection = Utility.SignedAngle(forward * transform.forward, forwardVelocity, transform.up) / 180.0f;
            float strength = rotationCorrectionStrength * forwardVelocity.magnitude;
            rigidBody.AddRelativeTorque(transform.up * rotationCorrection * strength, ForceMode.Acceleration);
        }

        // Add air friction to slow down the vehicle over time.
        Vector3 airForce = airFriction * Time.deltaTime;
        rigidBody.AddForce(Vector3.Scale(-rigidBody.velocity.normalized, airForce), ForceMode.VelocityChange);

        //hovering
        for (int i = 0; i < hoverPoints.Length; ++i)
        {
            Transform hoverPoint = hoverPoints[i];
            RaycastHit hit;
            Vector3 upVector;
            if (Physics.Raycast(hoverPoint.position, -hoverPoint.up, out hit, hoverHeight))
            {

                // Add force to make the vehicle hover over the ground.
                rigidBody.AddForceAtPosition(-hoverPoint.up * hoverForce * Mathf.Pow(hit.distance - hoverHeight, 3.0f), hoverPoint.position, ForceMode.Acceleration);
                upVector = hit.normal;
            }
            else
            {
                if (transform.position.y > hoverPoint.position.y)
                {
                    rigidBody.AddForceAtPosition(hoverPoint.up * tipOverStability, hoverPoint.position);
                }
                upVector = hoverPoint.up;
            }
            // Add force to stabilize the vehicle in the air.
            // http://answers.unity3d.com/questions/10425/how-to-stabilize-angular-motion-alignment-of-hover.html
            Vector3 predictedUp = Quaternion.AngleAxis(
                rigidBody.angularVelocity.magnitude * Mathf.Rad2Deg * hoverStability / hoverSpeed,
                rigidBody.angularVelocity
                ) * upVector;
            Vector3 torque = Vector3.Cross(predictedUp, upVector);
            rigidBody.AddTorque(torque * hoverSpeed * hoverSpeed, ForceMode.Acceleration);
        }
    }
}
