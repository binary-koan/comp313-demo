using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CharacterController))]
public class PlayerMovement : MonoBehaviour {
	public float speed = 3;
	public float rotateSpeed = 3;

	private CharacterController characterController;
	private bool moving4Directional = true;

	// Use this for initialization
	void Start() {
		characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		if (moving4Directional) {
			move4Directional();
		} else {
			rotateAndMove();
		}
	}

	void Update() {
		if (Input.GetKey(KeyCode.E)) {
			moving4Directional = !moving4Directional;
		}
	}

	void OnCollisionEnter(Collision collision) {
		var other = collision.gameObject;

		if (other.CompareTag("NPC")) {
			other.SetActive(false);
		}
	}

	private void move4Directional() {
		var forwardMovement = forward() * currentSpeed();
		var sidewaysMovement = transform.TransformDirection(Vector3.right) * currentRotation();
		var upwardMovement = Vector3.up * speed * 100;
		characterController.SimpleMove(forwardMovement + sidewaysMovement + upwardMovement);
	}

	private void rotateAndMove() {
		transform.Rotate(0, currentRotation(), 0);

		characterController.SimpleMove(forward() * currentSpeed());
	}

	private float currentRotation() {
		return rotateSpeed * Input.GetAxis("Horizontal");
	}

	private float currentSpeed() {
		return speed * Input.GetAxis("Vertical");
	}

	private Vector3 forward() {
		return transform.TransformDirection(Vector3.forward);
	}
}
