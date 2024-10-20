using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueMenuController : MonoBehaviour {
	[SerializeField] GameObject dialogueMenu;

	private HashSet<string> allowedScenes = new HashSet<string>() {
		"FirstFloor", "SecondFloor", "ThirdFloor", "FourthFloor", "Outside"
	};

	private void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (allowedScenes.Contains(scene.name)) {
			dialogueMenu.SetActive(true);
		}
		else {
			dialogueMenu.SetActive(false);
		}
	}
}
