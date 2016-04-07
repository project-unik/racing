using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class SpeedoMeter : MonoBehaviour
{
    private Rigidbody rigidBody;
    public Text speedoMeter;

    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Conversion from m/s to k/h is 3.6
        Vector3 forwardVelocity = Vector3.ProjectOnPlane(rigidBody.velocity, transform.up);
        speedoMeter.text = Mathf.FloorToInt(forwardVelocity.magnitude * 3.6f).ToString() + " kph";
    }
}
