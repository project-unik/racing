using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    public bool firstPerson;
    public float acceleration = 25;
    public float MaxSpeed = 100;
    public float maxBackwardSpeed = 50;
    [Tooltip("Rotation in degree per second")]
    public float rotationSpeed = 30;

    [Space]
    [Header("Scene Items")]
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;


    private Rigidbody rbPlayer;

    void Awake()
    {
        rbPlayer = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (firstPerson)
        {
            firstPersonCamera.enabled = true;
            thirdPersonCamera.enabled = false;
            thirdPersonCamera.gameObject.GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            firstPersonCamera.enabled = false;
            firstPersonCamera.gameObject.GetComponent<AudioListener>().enabled = false;
            thirdPersonCamera.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (Input.GetButton(InputManager.Accelerate))
        {
            Accelerate();
        }

        if (Input.GetButton(InputManager.Brake))
        {
            if (transform.InverseTransformDirection(rbPlayer.velocity).z > 0)
            {
                Brake();
            }
            if (transform.InverseTransformDirection(rbPlayer.velocity).z <= 0)
            {
                AccelerateBackwards();
            }
        }
    }

    private void Accelerate()
    {
        if (transform.InverseTransformDirection(rbPlayer.velocity).z < MaxSpeed)
        {
            rbPlayer.AddForce(transform.forward * acceleration);
        }
    }

    private void AccelerateBackwards()
    {
        if (transform.InverseTransformDirection(rbPlayer.velocity).z > -MaxSpeed)
        {
            rbPlayer.AddForce(transform.forward * -acceleration);
        }
    }

    private void Brake()
    {
        rbPlayer.AddForce(transform.forward * -acceleration);
        if (transform.InverseTransformDirection(rbPlayer.velocity).z <= 0)
        {
            rbPlayer.velocity = Vector3.zero;
        }
    }

}
