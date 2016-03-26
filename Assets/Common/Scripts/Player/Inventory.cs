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
        foreach (string pickup in inventory)
        {

        }
    }

    public void addPickup(string name)
    {
        if (inventory.Count < maxPickups)
        {
            Debug.logger.Log("Picked up a " + name);
            inventory.Enqueue(name);
        }
    }
}
