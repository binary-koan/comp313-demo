using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CharacterController))]
public class PlayerMovement : MonoBehaviour {
	public float speed = 3;
	public float pushStrength = 5;

	private CharacterController characterController;

	void Start() {
		characterController = GetComponent<CharacterController>();
	}

	void FixedUpdate() {
		var forwardMovement = transform.TransformDirection(Vector3.forward) * verticalSpeed();
		var sidewaysMovement = transform.TransformDirection(Vector3.right) * horizontalSpeed();

		characterController.SimpleMove(forwardMovement + sidewaysMovement);
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		var other = hit.gameObject;

		if (other.CompareTag("Pickup") && hit.rigidbody) {
			var pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

			other.GetComponent<Rigidbody>().AddForce(pushDirection * pushStrength);
		}
	}

	private float horizontalSpeed() {
		return speed * Input.GetAxis("Horizontal");
	}

	private float verticalSpeed() {
		return speed * Input.GetAxis("Vertical");
	}
}
