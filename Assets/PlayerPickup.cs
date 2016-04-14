using UnityEngine;
using System.Collections.Generic;

public class PlayerPickup : MonoBehaviour {
	public int grabDistance = 3;

	private List<GameObject> inventory = new List<GameObject>();

	void Update () {
		var clickedObject = findClickedObject();

		if (clickedObject && canPickUp(clickedObject) && isBeside(clickedObject)) {
			inventory.Add(clickedObject);
			clickedObject.SetActive(false);
		}
	}

	private GameObject findClickedObject() {
		if (Input.GetMouseButtonDown(0)) {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100)) {
				return hit.collider.gameObject;
			}
		}

		return null;
	}

	private bool canPickUp(GameObject item) {
		return item.CompareTag("Bouncer");
	}

	private bool isBeside(GameObject other) {
		return (other.transform.position - transform.position).magnitude <= grabDistance;
	}
}
