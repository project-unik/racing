using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
    private int maxPickups = 2;
    private Queue inventory;

    // Use this for initialization
    void Start()
    {
        inventory = new Queue(maxPickups);
    }

    // Update is called once per frame
    void Update()
    {
        if(inventory.Count>0 && Input.GetButtonUp(Tags.Input.FIRE))
        {
            Debug.logger.Log("Using a " + inventory.Dequeue());
        }
    }

    void OnGUI()
    {
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
    public bool addPickup(string pickupName)
    {
        if (inventory.Count < maxPickups)
        {
            Debug.logger.Log("Picked up a " + pickupName);
            inventory.Enqueue(pickupName);
            return true;
        }
        return false;
    }
}
