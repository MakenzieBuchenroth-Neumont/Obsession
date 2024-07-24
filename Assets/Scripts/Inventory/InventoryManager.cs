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

		//Cache the Inventory slot ItemData from InventoryManager
		ItemData toolToEquip = tools[slotIndex];

		//Change the Inventory Slot to the Hand's
		tools[slotIndex] = equippedTool;

		//Change the Hand's Slot to the Inventory Slot's
		equippedTool = toolToEquip;

		//Update the changes to the UI
		UIManager.Instance.renderInventory();

		// equip the tool
		equipTool(toolToEquip);
	}

	//Handles movement of item from Hand to Inventory
	public void handToInventory() {
		//Iterate through each inventory slot and find an empty slot
		for (int i = 0; i < tools.Length; i++) {
			if (tools[i] == null) {
				//Send the equipped item over to its new slot
				tools[i] = equippedTool;
				//Remove the item from the hand
				equippedTool = null;
				unequipTool();
				break;
			}
		}
		//Update changes in the inventory
		UIManager.Instance.renderInventory();
	}

	private void equipTool(ItemData toolToEquip) {
		if (currentToolObject != null) {
			Destroy(currentToolObject);
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
		}
	}
}