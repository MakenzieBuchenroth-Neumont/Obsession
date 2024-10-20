using System;
using System.Collections;
using System.Collections.Generic;
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
		ItemData toolToEquip = tools[slotIndex];

		if (toolToEquip != null) {
			handToInventory();

			equippedTool = toolToEquip;
			tools[slotIndex] = null;

			equipTool(toolToEquip);
			UIManager.Instance.renderInventory();
		}
		else {
			Debug.Log("No tool to equip from inventory!");
		}
	}

	public void handleEquippedTool() {
		if (equippedTool != null) {
			if (isInventoryFull()) {
				Debug.Log("Inventory is full. Dropping the item in hand.");
				dropHeldItem();
			}
			else {
				Debug.Log("Moving held item to inventory.");
				handToInventory();
			}
		}
	}

	private void dropHeldItem() {
		if (equippedTool != null) {
			GameObject droppedObject = Instantiate(equippedTool.gameModel, playerHand.position, Quaternion.identity);
			droppedObject.AddComponent<Rigidbody>();

			equippedTool = null;
			unequipTool();

			UIManager.Instance.renderInventory();
			Debug.Log($"Dropped {droppedObject.name} into the game world.");
		}
	}

	private void equipTool(ItemData toolToEquip) {
		if (currentToolObject != null) {
			Destroy(currentToolObject);
		}

		if (toolToEquip != null && toolToEquip.gameModel != null) {
			currentToolObject = Instantiate(toolToEquip.gameModel, playerHand);
			currentToolObject.transform.localPosition = toolPositions.TryGetValue(toolToEquip.name, out Vector3 position) ? position : Vector3.zero;
			currentToolObject.transform.localRotation = toolRotations.TryGetValue(toolToEquip.name, out Quaternion rotation) ? rotation : Quaternion.identity;
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
			droppedItem.AddComponent<Rigidbody>();

			equippedTool = null;
			unequipTool();
			UIManager.Instance.renderInventory();
			Debug.Log($"Dropped item: {droppedItem.name} at position {dropPos}.");
		}
		else {
			Debug.Log("No item to drop!");
		}
	}

	public bool isInventoryFull() {
		foreach (var tool in tools) {
			if (tool == null) return false;
		}
		return true;
	}

	public bool isHandFull() {
		return equippedTool != null;
	}

	public bool tryAddToInventory(ItemData item) {
		if (System.Array.Exists(tools, tool => tool == item)) {
			Debug.Log($"{item.name} is already in inventory!");
			return false;
		}

		for (int i = 0; i < tools.Length; i++) {
			if (tools[i] == null) {
				tools[i] = item;
				Debug.Log($"Added {item.name} to inventory.");
				UIManager.Instance.renderInventory();
				return true;
			}
		}
		Debug.Log("No empty inventory slot!");
		return false;
	}

	public void equipItemToHand(ItemData item) {
		if (equippedTool != null) {
			Debug.Log("Hand already has an item!");
			return;	
		}

		equippedTool = item;
		equipTool(item);
		Debug.Log("Item equipped to hand.");
	}

	public void handToInventory() {
		if (equippedTool == null) {
			Debug.Log("No tool is equipped.");
			return;
		}

		for (int i = 0; i < tools.Length; i++) {
			if (tools[i] == null) {
				tools[i] = equippedTool;
				equippedTool = null;
				unequipTool();
				Debug.Log($"Moved {tools[i].name} to inventory slot {i}.");
				UIManager.Instance.renderInventory();
				return;
			}
		}
		dropHeldItem();
	}
}