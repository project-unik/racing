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

    public float hoverForce = 2f;
    public float hoverStability = 0.3f;
    public float hoverSpeed = 2.0f;
    public float hoverHeight = 2f;

    public float airFriction = 10f;

    public float accForward = 1100;
    public float accBackward = 700f;

    public float turnStrength = 25f;
    public float slowDownTurn = 12f;

    private Transform[] hoverPoints;

    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        int i = 0;
        hoverPoints = new Transform[4];
        foreach(Transform child in transform)
        {
            hoverPoints[i++] = child;
        }
    }

    void Update()
    {
        // Handle player input for acceleration.
        float vertical = Input.GetAxis("Vertical");
        if (vertical > accDeadZone)
        {
            //set thrust forward
            curThrust = vertical * accForward;
        }
        else if (vertical < -accDeadZone)
        {
            //set thrust backward
            curThrust = vertical * accBackward;
        }
        else
        {
            //turn off thrust
            curThrust = 0;
        }

        float horizontal = Input.GetAxis("Horizontal");
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
    }

    void FixedUpdate()
    {
        if (curThrust != 0)
        {
            //apply force forward/backward
            rigidBody.AddForce(transform.forward * curThrust * Time.deltaTime, ForceMode.Acceleration);
        }

        if (curTurn != 0)
        {
            //turn the racer
            rigidBody.AddRelativeTorque(transform.up * curTurn * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            //slow down the sideward rotation
            Vector3 angVel = rigidBody.angularVelocity;
            angVel.y = angVel.y - angVel.y * slowDownTurn * Time.deltaTime;
            rigidBody.angularVelocity = angVel;
        }

        // Add air friction to slow down the vehicle over time.
        float airForce = airFriction * Time.deltaTime;
        rigidBody.AddForce(Vector3.Scale(-rigidBody.velocity.normalized, new Vector3(airForce, airForce, airForce)), ForceMode.VelocityChange);

        //hovering
        for (int i = 0; i < hoverPoints.Length; ++i)
        {
            Transform hoverPoint = hoverPoints[i];
            RaycastHit hit;
            Vector3 upVector;
            if (Physics.Raycast(hoverPoint.position, -hoverPoint.up, out hit,hoverHeight))
            {

                // Add force to make the vehicle hover over the ground.
                rigidBody.AddForceAtPosition(-hoverPoint.up * hoverForce * Mathf.Pow(hit.distance - hoverHeight, 3.0f), hoverPoint.position, ForceMode.Acceleration);
                upVector = hit.normal;
            }
            else
            {
                if (transform.position.y > hoverPoint.position.y)
                {
                    rigidBody.AddForceAtPosition(hoverPoint.up * hoverForce, hoverPoint.position);
                }
                else
                {
                    rigidBody.AddForceAtPosition(hoverPoint.up * -hoverForce, hoverPoint.position);
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
