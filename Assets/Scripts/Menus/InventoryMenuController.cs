using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryMenuController : MonoBehaviour {
	[SerializeField] GameObject inventoryMenu;

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
			inventoryMenu.SetActive(true);
		}
		else {
			inventoryMenu.SetActive(false);
		}
	}

	void Update() {
		if (allowedScenes.Contains(SceneManager.GetActiveScene().name)) {
			if (Input.GetKeyDown(KeyCode.I)) {
				inventoryMenu.SetActive(!inventoryMenu.activeSelf);
			}
		}
	}
}
