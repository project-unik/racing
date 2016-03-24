using UnityEngine;
using UnityEngine.UI;

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
        speedoMeter.text = Mathf.RoundToInt(forwardVelocity.magnitude * 3.6f).ToString() + " kph";
    }
}
