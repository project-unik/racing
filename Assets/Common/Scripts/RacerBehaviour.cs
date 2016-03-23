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
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RacerBehaviour : MonoBehaviour
{
    private Rigidbody rigidBody;
    private float accDeadZone = 0.1f;
    private float curThrust = 0.0f;
    private float curTurn = 0.0f;
    private Transform[] hoverPoints;
    private bool isGoingForward;

    public float hoverForce = 2f;
    public float hoverStability = 0.3f;
    public float hoverSpeed = 2.0f;
    public float hoverHeight = 2f;
    public float tipOverStability = 100.0f;

    public Vector3 airFriction = new Vector3(5f, 5f, 7f);

    public float maxSpeed = 100f;
    public float accForward = 1100;
    public float accBackward = 700f;

    public float turnStrength = 25f;
    public float slowDownTurn = 12f;
    [Range(0.0f, 2.0f)]
    public float turnSharpness = 0.8f;
    public float turnSlowdown = 5.0f;

    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        int i = 0;
        hoverPoints = new Transform[4];
        foreach (Transform child in transform)
        {
            hoverPoints[i++] = child;
        }
    }

    void Update()
    {
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
            curTurn = horizontal * turnStrength;
        }
        else
        {
            //do not turn sideward
            curTurn = 0;
        }

        isGoingForward = Mathf.Abs(Vector3.Angle(transform.forward, rigidBody.velocity)) <= 80.0f && rigidBody.velocity.magnitude > 0.0f;
    }

    void FixedUpdate()
    {
        // going forward
        if (curThrust >= 0f && rigidBody.velocity.magnitude <= maxSpeed)
        {
            //apply force forward/backward
            rigidBody.AddForce(transform.forward * curThrust * Time.deltaTime, ForceMode.Acceleration);
        }
        // braking
        else if (curThrust < 0f && isGoingForward)
        {
            rigidBody.AddForce(transform.forward * curThrust * Mathf.Pow(rigidBody.velocity.magnitude, 2.0f) * Time.fixedDeltaTime);
        }
        // going backwards
        else
        {
            rigidBody.AddForce(transform.forward * curThrust * Time.deltaTime, ForceMode.Acceleration);
        }

        if (curTurn != 0)
        {
            //turn the racer
            rigidBody.AddRelativeTorque(transform.up * curTurn * Time.deltaTime, ForceMode.Acceleration);
            Vector3 newDirection;
            // interpolate between current velocity direction and current forward transform
            if (turnSharpness <= 1.0f)
            {
                newDirection = (turnSharpness * transform.forward.normalized) + ((1.0f - turnSharpness) * rigidBody.velocity.normalized);
            }
            // turnSharpness is greater than 1 so the vehicle oversteers thus the current velocity direction is not relevant
            else
            {
                newDirection = turnSharpness * transform.forward.normalized;
            }

            newDirection = newDirection.normalized;
            float turnAngle = Mathf.Abs(Vector3.Angle(newDirection, rigidBody.velocity));
            // vehicle is slowed down when turning depending on angle
            rigidBody.velocity = newDirection * rigidBody.velocity.magnitude * (1.0f - (turnAngle / 360.0f) * turnSlowdown);
        }
        else
        {
            //slow down the sideward rotation
            Vector3 angVel = rigidBody.angularVelocity;
            angVel.y = angVel.y - angVel.y * slowDownTurn * Time.deltaTime;
            rigidBody.angularVelocity = angVel;
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
