using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public GameObject player;

    private Vector3 offset;

	void Start() {
        offset = transform.position - player.transform.position;
	}
	
	// LateUpdate ensures that the camera moves after the player
	void LateUpdate() {
        transform.position = player.transform.position + offset;
	}
}
