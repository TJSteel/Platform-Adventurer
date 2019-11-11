using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float cameraOffsetLeft;
    public float minX;
    public float maxX;

    void LateUpdate() {

        float playerX = player.transform.position.x;
        float cameraX = transform.position.x;

        if (playerX > cameraX && playerX <= maxX)
        {
            transform.position = new Vector3(playerX, transform.position.y, transform.position.z);
        }
        else if (playerX < (cameraX - cameraOffsetLeft) && (playerX + cameraOffsetLeft >= minX))
        {
            transform.position = new Vector3(playerX + cameraOffsetLeft, transform.position.y, transform.position.z);
        }
    }
}
