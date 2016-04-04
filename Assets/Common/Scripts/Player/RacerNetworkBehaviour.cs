using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.Networking;

public class RacerNetworkBehaviour : NetworkBehaviour
{
    private GameObject cam;
    /// <summary>
    /// The camera which should be spawned with the player
    /// </summary>
    public GameObject cameraPrefab;

    public override void OnStartLocalPlayer()
    {
        Assert.IsNotNull(cameraPrefab, "Camera prefab for player must not be null.");
        Debug.logger.Log("Spawning local player...");
        GetComponent<MeshRenderer>().material.color = Color.red;
        Debug.logger.Log("Spawning camera for local player...");
        cam = (GameObject)Instantiate(cameraPrefab, transform.position, Quaternion.identity);
        cam.GetComponent<CameraBehaviour>().setTrackedObject(gameObject);
    }

    void OnDestroy()
    {
        Debug.logger.Log("Destroying camera for local player...");
        Destroy(cam);
    }
}
