using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Tool")]
public class EquipmentData : ItemData {
	public enum EquipmentType {
		Knife,
		Scissors,
		Bat,
		Hand
	}
	public EquipmentType equipmentType;

}
