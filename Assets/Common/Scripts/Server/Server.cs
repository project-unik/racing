using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    public NetworkManager manager;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Starting server...");
        manager.StartServer();
    }
}
