using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class PlayerInventory : MonoBehaviour {
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

    public void DropItem(BaseEventData data) {
        var pointerData = (PointerEventData)data;
        var hudItem = pointerData.pointerPress;

        var inventoryItem = inventory.Find((item) => item.hudItem == hudItem);
        if (inventoryItem == null) {
            Debug.LogWarning("Trying to drop nonexistent game object.", hudItem);
            return;
        }

        dropFromInventory(inventoryItem);
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
		return item.CompareTag("Pickup");
	}

    private bool isBeside(GameObject item) {
		return (item.transform.position - transform.position).magnitude <= grabDistance;
	}

	private void addToInventory(GameObject item) {
		item.SetActive(false);

		var hudItem = (GameObject)Instantiate(inventoryItemDisplay, Vector3.zero, Quaternion.identity);
		hudItem.SetActive(true);
		hudItem.transform.SetParent(inventoryPanel.transform, false);
        setHudItemPosition(hudItem, inventory.Count);

        inventory.Add(new InventoryItem(item, hudItem));
	}

    private void dropFromInventory(InventoryItem inventoryItem) {
        var index = inventory.IndexOf(inventoryItem);
        inventory.RemoveAt(index);

        Destroy(inventoryItem.hudItem);

        inventoryItem.item.transform.localEulerAngles = Vector3.zero;
        inventoryItem.item.transform.position = transform.position + Vector3.up + transform.TransformDirection(Vector3.forward);
        inventoryItem.item.GetComponent<Rigidbody>().velocity = Vector3.up + transform.TransformDirection(Vector3.forward);
        inventoryItem.item.SetActive(true);

        for (var i = index; i < inventory.Count; i++) {
            setHudItemPosition(inventory[i].hudItem, i);
        }
    }

    private void setHudItemPosition(GameObject hudItem, int index) {
        var itemTransform = (RectTransform)(hudItem.transform);

        itemTransform.anchoredPosition = new Vector2(-150, -50 - 70 * index);
    }
}
