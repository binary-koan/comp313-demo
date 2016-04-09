using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof (PlayerDash))]
public class PlayerMovement : MonoBehaviour {
	public float speed = 3;
	public float pushStrength = 5;

	public float dashSpeedMultiplier = 2;
	public float dashPushMultiplier = 10;

	private CharacterController characterController;
	private PlayerDash dashController;

	void Start() {
		characterController = GetComponent<CharacterController>();
		dashController = GetComponent<PlayerDash>();
	}

	void FixedUpdate() {
		var movement =
			transform.TransformDirection(Vector3.forward) * verticalSpeed() +
			transform.TransformDirection(Vector3.right) * horizontalSpeed();
		
		if (dashController.isDashing) {
			movement *= dashSpeedMultiplier;
		}

		characterController.SimpleMove(movement);
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		var other = hit.gameObject;

		if (other.CompareTag("Bouncer") && hit.rigidbody) {
			var push = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z) * pushStrength;

			if (dashController.isDashing) {
				// Push the object harder and slightly upwards
				push.y = 5;
				push *= dashPushMultiplier;
			}

			other.GetComponent<Rigidbody>().AddForce(push);
		}
	}

	private float horizontalSpeed() {
		return speed * Input.GetAxis("Horizontal");
	}

	private float verticalSpeed() {
		return speed * Input.GetAxis("Vertical");
	}
}
