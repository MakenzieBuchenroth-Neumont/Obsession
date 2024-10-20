using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class Quest : ScriptableObject {
	public string questGiver;
	public string objective;
	public string description;

	public string itemToFind;
	public bool isCompleted;

	public void completeQuest() {
		isCompleted = true;
		objective = $"Give {questGiver} the {itemToFind}.";
	}
}
