using UnityEngine;
using System.Collections;

public class WanderMovement : MonoBehaviour {
	public float speed = 5;
	public float directionChangeInterval = 1;
	public float maxHeadingChange = 30;

	CharacterController controller;
	float heading;
	Vector3 targetRotation;

	void Awake() {
		heading = Random.Range(0, 360);

		StartCoroutine(NewHeading());
	}

	void Start() {
		controller = GetComponent<CharacterController>();

		// Set random initial rotation
		transform.eulerAngles = new Vector3(0, heading, 0);
	}

	void FixedUpdate() {
		transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
		var forward = transform.TransformDirection(Vector3.forward);
		controller.SimpleMove(forward * speed);
	}
		
	IEnumerator NewHeading() {
		while (true) {
			NewHeadingRoutine();
			yield return new WaitForSeconds(directionChangeInterval);
		}
	}

	void NewHeadingRoutine() {
		var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
		var ceil  = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
		heading = Random.Range(floor, ceil);
		targetRotation = new Vector3(0, heading, 0);
	}
}
