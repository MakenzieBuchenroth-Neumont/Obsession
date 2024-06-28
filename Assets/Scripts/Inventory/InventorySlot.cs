using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler {
	ItemData itemToDisplay;
	public Image itemDisplayImage;

	int slotIndex;

	public void Display(ItemData itemToDisplay) {
		//Check if there is an item to display
		if (itemToDisplay != null) {
			//Switch the thumbnail over
			itemDisplayImage.sprite = itemToDisplay.thumbnail;
			this.itemToDisplay = itemToDisplay;

			itemDisplayImage.gameObject.SetActive(true);

			return;
		}

		itemDisplayImage.gameObject.SetActive(false);
	}

	public virtual void OnPointerClick(PointerEventData eventData) {
		//Move item from inventory to hand
		InventoryManager.Instance.inventoryToHand(slotIndex);
	}

	//Set the Slot index
	public void AssignIndex(int slotIndex) {
		this.slotIndex = slotIndex;
	}
}