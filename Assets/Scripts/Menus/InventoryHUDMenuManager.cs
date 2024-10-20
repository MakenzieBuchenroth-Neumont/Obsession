using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryHUDMenuController : MonoBehaviour {
	[SerializeField] GameObject hudMenu;

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
			hudMenu.SetActive(true);
		}
		else {
			hudMenu.SetActive(false);
		}
	}
}
