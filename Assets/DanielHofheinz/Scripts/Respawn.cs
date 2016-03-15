using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour {

    private Transform startPosition;
    private Vector3 respawnPoint = new Vector3(-10000,-10000,-10000);

    IEnumerator RespawnCountdown()
    {
        Rigidbody rbPlayer = gameObject.GetComponent<Rigidbody>();
        rbPlayer.velocity = Vector3.zero;
        rbPlayer.useGravity = false;
        GameManager.Moveable = false;
        yield return new WaitForSeconds(3);
        rbPlayer.useGravity = true;
        GameManager.Moveable = true;
    }

    void Start()
    {
        startPosition = transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TagManager.DeathZone)
        {
            if (respawnPoint != new Vector3(-10000, -10000, -10000))
            {
                transform.position = respawnPoint;
            }
            else
            {
                transform.position = startPosition.position;
            }
            StartCoroutine(RespawnCountdown());
        }
    }

    void OnTriggerExit(Collider other)
    {
        RespawnPoint respawnPosition = other.GetComponent<RespawnPoint>();
        if(respawnPosition == null)
        {
            return;
        }
        Debug.Log(respawnPosition.GetRespawnPosition(), this);
        respawnPoint = respawnPosition.GetRespawnPosition();
    }

}
