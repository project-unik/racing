using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(Collider))]
public abstract class Pickup : NetworkBehaviour
{
    /// <summary>
    /// Performs the action of this pickup. It is not allowed to change the state of the pickup.
    /// </summary>
    /// <param name="player"></param>
    abstract public void Use(GameObject player);

    void OnTriggerEnter(Collider other)
    {
        if(!isServer)
        {
            return;
        }
        if (other.tag == Tags.GameObjects.PLAYER && other.gameObject.GetComponent<Inventory>().AddPickup(this.GetType().Name))
        {
            Destroy(gameObject);
        }
    }
}
