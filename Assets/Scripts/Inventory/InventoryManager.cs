using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
	public static InventoryManager Instance { get; private set; }

	private void Awake() {
		//If there is more than one instance, destroy the extra
		if (Instance != null && Instance != this) {
			Destroy(this);
		}
		else {
			//Set the static instance to this instance
			Instance = this;
		}

		Debug.Log($"Inventory initialized with {tools.Length} slots.");
	}

	[Header("Tools")]
	//Tool Slots
	public ItemData[] tools = new ItemData[8];
	//Tool in the player's hand
	public ItemData equippedTool = null;

	[Header("Items")]
	//Item Slots
	public ItemData[] items = new ItemData[2];
	//Item in the player's hand
	public ItemData equippedItem = null;

	[Header("Player")]
	// reference to the player's hand transform
	public Transform playerHand;

	private Dictionary<string, Vector3> toolPositions = new Dictionary<string, Vector3> {
		{ "Knife", new Vector3(0.007f, 0.063f, 0.081f) }, // Adjust these values as needed
        { "Scissors", new Vector3(-0.095f, 0.12f, 0.038f) },
		{ "BaseballBat", new Vector3(-0.095f, 0.112f, 0.386f) }
	};

	private Dictionary<string, Quaternion> toolRotations = new Dictionary<string, Quaternion> {
		{ "Knife", Quaternion.Euler(-98.917f, 45.451f, -155.636f) },
		{ "Scissors", Quaternion.Euler(-11.779f, -99.472f, 135.209f) },
		{ "BaseballBat", Quaternion.Euler(4.132f, -197.704f, -102.925f) }
	};

	// the currently instantiated tool object
	private GameObject currentToolObject;

	//Equipping
	//Handles movement of item from Inventory to Hand
	public void inventoryToHand(int slotIndex) {
		if (slotIndex < 0 || slotIndex >= tools.Length) {
			Debug.LogError("Invalid slot index!");
			return;
		}

		//Cache the Inventory slot ItemData from InventoryManager
		ItemData toolToEquip = tools[slotIndex];

		if (toolToEquip != null) {
			if (equippedTool != null) {
				for (int i = 0; i < tools.Length; i++) {
					if (tools[i] == null) {
						tools[i] = equippedTool;
						equippedTool = null;
						break;
				}
			}
		}

			//Change the Hand's Slot to the Inventory Slot's
			equippedTool = toolToEquip;

			//Change the Inventory Slot to the Hand's
			tools[slotIndex] = null;

			// equip the tool
			equipTool(toolToEquip);

			//Update the changes to the UI
			UIManager.Instance.renderInventory();
		}
		else {
			Debug.Log("No tool to equip from inventory!");
		}

	}

	//Handles movement of item from Hand to Inventory
	public void handToInventory() {
		if (equippedTool == null) {
			Debug.Log("No tool is equipped.");
			return;
		}
		for (int i = 0; i < tools.Length; i++) {
			if (tools[i] == null) {
				tools[i] = equippedTool;
				unequipTool();
				equippedTool = null;
				Debug.Log($"Moved {tools[i].name} to inventory slot {i}.");
				break;
			}
		}

		if (equippedTool != null) {
			Debug.Log("No empty inventory slot!");
		}
		UIManager.Instance.renderInventory();
	}

	private void equipTool(ItemData toolToEquip) {
		if (currentToolObject != null) {
			Destroy(currentToolObject);
		}

		Debug.Log("Current Inventory State:");
		for (int i = 0; i < tools.Length; i++) {
			Debug.Log($"Slot {i}: {tools[i]?.name ?? "Empty"}");
		}

		if (toolToEquip != null && toolToEquip.gameModel != null) {
			// Instantiate the tool and parent it to the player's hand
			currentToolObject = Instantiate(toolToEquip.gameModel, playerHand);

			// Reset position and rotation
			currentToolObject.transform.localPosition = Vector3.zero;
			currentToolObject.transform.localRotation = Quaternion.identity;

			// Apply specific position and rotation if defined
			if (toolPositions.TryGetValue(toolToEquip.name, out Vector3 position)) {
				currentToolObject.transform.localPosition = position;
			}

			if (toolRotations.TryGetValue(toolToEquip.name, out Quaternion rotation)) {
				currentToolObject.transform.localRotation = rotation;
			}
		}
	}

	private void unequipTool() {
		if (currentToolObject != null) {
			Destroy(currentToolObject);
			currentToolObject = null;
		}
	}

	public void dropItem() {
		if (equippedTool != null) {
			Vector3 dropPos = playerHand.position + Vector3.down * 0.5f;
			GameObject droppedItem = Instantiate(equippedTool.gameModel, dropPos, Quaternion.identity);

			Debug.Log($"Dropped item: {equippedTool.name} at position: {dropPos}");

			Rigidbody rb = droppedItem.AddComponent<Rigidbody>();

			equippedTool = null;

			if (currentToolObject != null) {
				Destroy(currentToolObject);
				currentToolObject = null;
			}

			UIManager.Instance.renderInventory();
		}
		else {
			Debug.Log("No item to drop!");
		}
	}

	//public void addItemToInventory(ItemData itemToAdd) {
	//	Debug.Log($"Trying to add {itemToAdd.name} to inventory");

	//	for (int i = 0; i < tools.Length; i++) {
	//		if (tools[i] == itemToAdd) {
	//			Debug.Log($"{itemToAdd.name} is already in the inventory.");
	//			return;
	//		}
	//	}

	//	for (int i = 0; i < tools.Length; i++) {
	//		if (tools[i] == null) {
	//			tools[i] = itemToAdd;
	//			Debug.Log($"Added {itemToAdd.name} to inventory in slot {i}");
	//			UIManager.Instance.renderInventory();
	//			return;
	//		}
	//	}
	//	Debug.Log("No empty inventory slot available!");
	//}

	public bool isInventoryFull() {
		for (int i = 0; i < tools.Length; i++) {
			if (tools[i] == null) {
				return false;
			}
		}
			return true;
		}

		public bool tryAddToInventory(ItemData item) {
			for (int i = 0; i < tools.Length; i++) {
				if (tools[i] == item) {
					Debug.Log($"{item.name} is already in inventory!");
					return false;
				}
			}

			for (int i = 0; i < tools.Length; i++) {
				if (tools[i] == null) {
					tools[i] = item;
					Debug.Log($"Added {item.name} to inventory");
					UIManager.Instance.renderInventory();
					return true;
				}
			}

			Debug.Log("No empty inventory slot!");
			return false;
		}

	public bool isHandFull() {
		return equippedItem != null;
	}
}