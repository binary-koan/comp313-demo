using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerPickup : MonoBehaviour {
	public int grabDistance = 3;
	public GameObject inventoryPanel;
	public GameObject inventoryItemDisplay;

	private class InventoryItem {
		public GameObject item { get; private set; }
		public GameObject hudItem { get; private set; }

		public InventoryItem(GameObject item, GameObject hudItem) {
			this.item = item;
			this.hudItem = hudItem;
		}
	}

	private List<InventoryItem> inventory = new List<InventoryItem>();

	void Update () {
		var clickedObject = findClickedObject();

		if (clickedObject && canPickUp(clickedObject) && isBeside(clickedObject)) {
			addToInventory(clickedObject);
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

	private bool isBeside(GameObject item) {
		return (item.transform.position - transform.position).magnitude <= grabDistance;
	}

	private void addToInventory(GameObject item) {
		item.SetActive(false);

		var hudItem = (GameObject)Instantiate(inventoryItemDisplay, new Vector3(-150, -50 - 70 * inventory.Count, 0), Quaternion.identity);
		hudItem.SetActive(true);
		hudItem.transform.SetParent(inventoryPanel.transform, false);

		inventory.Add(new InventoryItem(item, hudItem));
	}
}
