using UnityEngine;
using System.Collections;
using System;

public class Boost : Pickup
{
    public override void Use(GameObject player)
    {
        player.GetComponent<Rigidbody>().AddForce(player.transform.forward*5000, ForceMode.Acceleration);
    }
}
