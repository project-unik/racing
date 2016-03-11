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

    public float hoverForce = 7f;
    public float hoverStability = 0.3f;
    public float hoverSpeed = 2.0f;
    public float hoverHeight = 2f;

    public float airFriction = 10f;

    public float accForward = 1100;
    public float accBackward = 700f;

    public float turnStrength = 16f;
    public float slowDownTurn = 12f;

    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
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
            rigidBody.AddForce(transform.forward * curThrust * Time.deltaTime);
        }

        if (curTurn != 0)
        {
            //turn the racer
            rigidBody.AddRelativeTorque(transform.up * curTurn * Time.deltaTime);
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
        RaycastHit hit;
        Vector3 upVector;
        if (Physics.Raycast(this.transform.position, -transform.up, out hit))
        {

            // Add force to make the vehicle hover over the ground.
            // http://answers.unity3d.com/questions/46024/maintain-heightaltitudedistance-from-ground.html
            if (hit.distance < 2.0f * hoverHeight)
            {
                rigidBody.AddForce(-transform.up * hoverForce * Mathf.Pow(hit.distance - hoverHeight, 3.0f));
            }
            upVector = hit.normal;
        }
        else
        {
            upVector = transform.up;
        }

        // Add force to stabilize the vehicle in the air.
        // http://answers.unity3d.com/questions/10425/how-to-stabilize-angular-motion-alignment-of-hover.html
        Vector3 predictedUp = Quaternion.AngleAxis(
            rigidBody.angularVelocity.magnitude * Mathf.Rad2Deg * hoverStability / hoverSpeed,
            rigidBody.angularVelocity
            ) * transform.up;
        Vector3 torque = Vector3.Cross(predictedUp, upVector);
        rigidBody.AddTorque(torque * hoverSpeed * hoverSpeed);

    }
}
