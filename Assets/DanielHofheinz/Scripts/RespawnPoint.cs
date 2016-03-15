using UnityEngine;
using System.Collections;

public class RespawnPoint : MonoBehaviour {

    public float yOffset = 3.0f;

    private Vector3 respawnPos;

    void Start()
    {
        respawnPos = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
    }

    public Vector3 GetRespawnPosition()
    {
        return respawnPos;
    }

}
