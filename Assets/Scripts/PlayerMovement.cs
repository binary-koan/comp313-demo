using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof (PlayerDash))]
public class PlayerMovement : MonoBehaviour {
	public float speed = 3;
	public float pushStrength = 5;

	public float dashSpeedMultiplier = 2;
	public float dashPushMultiplier = 10;

	private CharacterController characterController;
	private PlayerDash dashController;
    private Transform modelTransform;
    private Animator modelAnimator;

	void Start() {
		characterController = GetComponent<CharacterController>();
		dashController = GetComponent<PlayerDash>();
        modelTransform = transform.GetChild(0);
        modelAnimator = GetComponentInChildren<Animator>();
	}

	void Update() {
		var movement = Vector3.forward * verticalSpeed() + Vector3.right * horizontalSpeed();
		
		if (dashController.isDashing) {
			movement *= dashSpeedMultiplier;
		}

		characterController.SimpleMove(movement);

        if (movement.magnitude > 0) {
            transform.LookAt(transform.position + movement);
            modelAnimator.SetBool("IsMoving", true);
            modelAnimator.SetFloat("MovementSpeed", movement.magnitude);
        } else {
            modelAnimator.SetBool("IsMoving", false);
        }
	}

    void OnControllerColliderHit(ControllerColliderHit hit) {
		var other = hit.gameObject;

		if (other.CompareTag("Bouncer") && hit.rigidbody) {
			var push = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z) * pushStrength;

			if (dashController.isDashing) {
				// Push the object harder and slightly upwards
				push.y = pushStrength;
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
