using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class UIManager : MonoBehaviour, ITimeTracker {
	public static UIManager Instance { get; private set; }
	[SerializeField] public GameObject interactionPrompt;

	[Header("Status Bar")]
	// tool equip slot
	public Image toolEquipSlot;
	public Image toolEquipSlotParent;
	// time ui
	public TextMeshProUGUI timeText;
	public TextMeshProUGUI dateText;

	[Header("Inventory System")]
	// the inventory panel
	public GameObject inventoryPanel;

	// the tool equip slot
	public HandInventorySlot toolHandSlot;
	public Image toolHandSlotParent;

	// the tool slot ui
	public InventorySlot[] toolSlots;

	#region AwakeStartUpdate
	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private void Start() {
		renderInventory();
		AssignSlotIndexes();

		// add UIManager to the list of objects TimeManager will notify when the time updates
		TimeManager.Instance.registerTracker(this);
	}

	private void Update() {
		renderInventory();
	}
	#endregion

	#region Inventory
	public void AssignSlotIndexes() {
		for (int i = 0; i < toolSlots.Length; i++) {
			toolSlots[i].AssignIndex(i);
		}
	}

	//Render the inventory screen to reflect the Player's Inventory. 
	public void renderInventory() {
		//Get the inventory tool slots from Inventory Manager
		ItemData[] inventoryToolSlots = InventoryManager.Instance.tools;

		//Get the inventory item slots from Inventory Manager
		ItemData[] inventoryItemSlots = InventoryManager.Instance.items;

		//Render the Tool section
		renderInventoryPanel(inventoryToolSlots, toolSlots);

		//Render the Item section

		//Render the equipped slots
		toolHandSlot.Display(InventoryManager.Instance.equippedTool);

		//Get Tool Equip from InventoryManager
		ItemData equippedTool = InventoryManager.Instance.equippedTool;

		//Check if there is an item to display
		if (equippedTool != null) {
			//Switch the thumbnail over
			toolEquipSlot.sprite = equippedTool.thumbnail;

			toolEquipSlot.gameObject.SetActive(true);

			toolEquipSlotParent.color = Color.clear;
			//toolHandSlotParent.color = Color.clear;

			return;
		}

		toolEquipSlot.gameObject.SetActive(false);
		toolHandSlotParent.color = new Color(247, 185, 202);	
		toolEquipSlotParent.color = new Color(247, 185, 202);
	}

	//Iterate through a slot in a section and display them in the UI
	void renderInventoryPanel(ItemData[] slots, InventorySlot[] uiSlots) {
		for (int i = 0; i < uiSlots.Length; i++) {
			//Display them accordingly
			uiSlots[i].Display(slots[i]);
		}
	}

	public void toggleInventoryPanel() {
		inventoryPanel.SetActive(!inventoryPanel.activeSelf);
	}
	#endregion

	// callback to handle the UI for time
	public void clockUpdate(GameTimestamp timestamp) {
		//Handle the time
		//Get the hours and minutes
		int hours = timestamp.hour;
		int minutes = timestamp.minute;

		//AM or PM
		string prefix = "AM ";

		//Convert hours to 12 hour clock
		if (hours > 12) {
			//Time becomes PM 
			prefix = "PM ";
			if (hours > 12) {
				hours -= 12;
			}
		}
		else {
			prefix = "AM ";
		}

		if (hours == 0) {
			hours = 12;
		}

		//Format it for the time text display
		timeText.text = + hours + ":" + minutes.ToString("00") + prefix;

		// handle the day
		int day = timestamp.day;


		string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

		// format
		dateText.text = dayOfTheWeek;

	}

	#region Interaction Prompt
	public void showInteractionPrompt(string message) {
		if (interactionPrompt != null) {
			interactionPrompt.SetActive(true);
			TMP_Text textComponent = interactionPrompt.GetComponentInChildren<TMP_Text>();
			if (textComponent != null) {
				textComponent.text = message;
			}
			else {
				Debug.Log("No text component found.");
			}
		}
	}

	public void hideInteractionPrompt() {
		if (interactionPrompt != null) {
			interactionPrompt.SetActive(false);
		}
	}
	#endregion
}
