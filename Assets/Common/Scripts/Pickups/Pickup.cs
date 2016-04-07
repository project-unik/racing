using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(Collider))]
public abstract class Pickup : NetworkBehaviour
{
    /// <summary>
    /// Returns the name of the pickup.
    /// </summary>
    /// <returns></returns>
    abstract protected string GetName();

    void OnTriggerEnter(Collider other)
    {
        if(!isServer)
        {
            return;
        }
        if (other.tag == Tags.GameObjects.PLAYER && other.gameObject.GetComponent<Inventory>().AddPickup(GetName()))
        {
            Destroy(gameObject);
        }
    }
}
