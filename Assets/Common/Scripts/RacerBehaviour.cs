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

public class RacerBehaviour : MonoBehaviour
{
    private Rigidbody rb;

    public float repellStrength = 10.0f;
    public float hoverStability = 0.3f;
    public float hoverSpeed = 2.0f;
    public float airFriction = 1.0f;
    public float accForward = 20.0f;
    public float accBackward = 10.0f;
    public float accSide = 14.0f;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 upVector = Vector3.up;
        if (Physics.Raycast(this.transform.position, new Vector3(0, -1, 0), out hit))
        {
            // Add force to make the vehicle hover over the ground.
            // http://answers.unity3d.com/questions/46024/maintain-heightaltitudedistance-from-ground.html
            rb.AddForce(transform.up * (repellStrength / (hit.distance * 0.5f)));
            upVector = hit.normal;
        }

        // Add force to stabilize the vehicle in the air.
        // http://answers.unity3d.com/questions/10425/how-to-stabilize-angular-motion-alignment-of-hover.html
        Vector3 predictedUp = Quaternion.AngleAxis(
            rb.angularVelocity.magnitude * Mathf.Rad2Deg * hoverStability / hoverSpeed,
            rb.angularVelocity
            ) * transform.up;
        Vector3 torque = Vector3.Cross(predictedUp, upVector);
        rb.AddTorque(torque * hoverSpeed * hoverSpeed);

        // Add air friction to slow down the vehicle over time.
        float f = airFriction * Time.deltaTime;
        rb.AddForce(Vector3.Scale(-rb.velocity.normalized, new Vector3(f, f, f)), ForceMode.VelocityChange);

        // Handle player input for acceleration.
        float vertical = Input.GetAxis("Vertical");
        if (vertical != 0)
        {
            //accelerate forward
            if (vertical > 0)
            {
                rb.velocity += new Vector3(0, 0, vertical * accForward * Time.deltaTime);
            }
            //accelerate backward
            else
            {
                rb.velocity += new Vector3(0, 0, vertical * accBackward * Time.deltaTime);
            }
        }
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0)
        {
            //accelerate sideward
            rb.velocity += new Vector3(horizontal * accSide * Time.deltaTime, 0, 0);
        }
    }
}
