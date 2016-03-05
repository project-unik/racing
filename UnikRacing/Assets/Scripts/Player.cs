using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public bool firstPerson;
    public float speed = 1;
    [Tooltip("Rotation in degree per second")]
    public float rotationSpeed = 30;

    [Space]
    [Header("Scene Items")]
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

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

    void Update()
    {
        if (Input.GetButton(InputManager.Vertical))
        {
            float horizontal = Input.GetAxisRaw(InputManager.Horizontal);
            Vector3 rotation = Rotation(horizontal);
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x + rotation.x,
                transform.rotation.eulerAngles.y + rotation.y,
                transform.rotation.eulerAngles.z + rotation.z);
        }

        float vertical = Input.GetAxisRaw(InputManager.Vertical);
        transform.position += Movement(vertical);
    }

    private Vector3 Rotation(float horizontal)
    {
        float y = horizontal * rotationSpeed * Time.deltaTime;
        Vector3 rotation = new Vector3(0, y, 0);
        return rotation;
    }

    private Vector3 Movement(float vertical)
    {
        float x, z;
        x = Mathf.Sin(transform.rotation.eulerAngles.y / 180 * Mathf.PI);
        z = Mathf.Cos(transform.rotation.eulerAngles.y / 180 * Mathf.PI);
        Vector3 movement = new Vector3(x, 0, z);
        movement *= vertical * speed * Time.deltaTime;
        return movement;
    }

}
