using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item")]
public class ItemData : ScriptableObject {
	public string description;

	// UI Icon
	public Sprite thumbnail;

	//GameObject to be shown in scene
	public GameObject gameModel;

	public new string name;
}
