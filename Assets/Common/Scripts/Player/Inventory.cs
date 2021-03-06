﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Reflection;

public class Inventory : NetworkBehaviour
{
    private int maxPickups = 2;
    private SyncListString inventory = new SyncListString();

    private void InventoryChanged(SyncListString.Operation op, int itemIndex)
    {
        Debug.Log("Inventory changed, Operation " + op + " for itemIndex " + itemIndex);
    }

    void Start()
    {
        inventory.Callback = InventoryChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetButtonUp(Tags.Input.FIRE))
        {
            CmdUsePickup();
        }
    }

    [Command]
    void CmdUsePickup()
    {
        if (inventory.Count > 0)
        {
            string pickup = inventory[0];
            inventory.RemoveAt(0);
            //temp. implementation
            System.Type type = System.Type.GetType(pickup);
            MethodInfo useMethod = type.GetMethod("Use");
            useMethod.Invoke(System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type), new Object[] { gameObject });
        }
    }

    void OnGUI()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        int i = 1;
        foreach (string pickup in inventory)
        {
            GUI.Label(new Rect(10, 15 * i, 100, 20), i++ + ". " + pickup);
        }
    }

    /// <summary>
    /// Tries to add the passed pickup to the inventory if possible (the inventory is not full, etc.).
    /// </summary>
    /// <param name="name"></param>
    /// <returns>True if the pickup was added to the inventory, false otherwise .</returns>
    public bool AddPickup(string pickupName)
    {
        if (inventory.Count < maxPickups)
        {
            inventory.Add(pickupName);
            return true;
        }
        return false;
    }
}