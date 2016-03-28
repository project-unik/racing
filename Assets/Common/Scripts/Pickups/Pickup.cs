using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public abstract class Pickup : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Returns the name of the pickup.
    /// </summary>
    /// <returns></returns>
    abstract protected string getName();

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.GameObjects.PLAYER && other.gameObject.GetComponent<Inventory>().addPickup(getName()))
        {
            Destroy(gameObject);
        }
    }
}
