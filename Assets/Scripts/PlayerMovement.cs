using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof (CharacterController))]
public class PlayerMovement : MonoBehaviour {
	public float speed = 3;
	public float pushStrength = 5;

	public float dashSpeedMultiplier = 2;
	public float dashPushMultiplier = 10;

	public Text dashDisplay;

	private CharacterController characterController;

	private const int MAX_DASH = 50;
	private int dashRemaining = MAX_DASH;
	private bool dashRecharging = false;

	void Start() {
		characterController = GetComponent<CharacterController>();
	}

	void FixedUpdate() {
		var movement =
			transform.TransformDirection(Vector3.forward) * verticalSpeed() +
			transform.TransformDirection(Vector3.right) * horizontalSpeed();
		
		if (isDashing()) {
			movement *= dashSpeedMultiplier;

			dashRecharging = false;
			dashRemaining--;
		} else if (!dashRecharging && (dashRemaining == 0 || (!isDashing() && dashRemaining < MAX_DASH))) {
			dashRecharging = true;
		} else if (dashRecharging && dashRemaining < MAX_DASH) {
			dashRemaining++;

			if (dashRemaining == MAX_DASH) {
				dashRecharging = false;
			}
		}

		characterController.SimpleMove(movement);
	}

	void Update() {
		dashDisplay.text = "Dash: " + dashRemaining.ToString();
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		var other = hit.gameObject;

		if (other.CompareTag("Pickup") && hit.rigidbody) {
			var push = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z) * pushStrength;

			if (isDashing()) {
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

	private bool isDashing() {
		return !dashRecharging && Input.GetKey(KeyCode.Space) && dashRemaining > 0;
	}
}
