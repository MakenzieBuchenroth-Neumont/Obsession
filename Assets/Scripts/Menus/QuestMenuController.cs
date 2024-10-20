using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestMenuController : MonoBehaviour {
	[SerializeField] GameObject questMenu;
	[SerializeField] GameObject pauseMenu;

	private HashSet<string> allowedScenes = new HashSet<string>() {
		"FirstFloor", "SecondFloor", "ThirdFloor", "FourthFloor", "Outside", "Bedroom"
	};

	private void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (allowedScenes.Contains(scene.name)) {
			questMenu.SetActive(false);
		}
		else {
			questMenu.SetActive(true);
		}
	}

	public void quest() {
		if (GameManager.gameMode == GameManager.GameMode.Game) {
			Debug.Log("Clicked yearbook");
			questMenu.SetActive(!questMenu.activeSelf);
			if (pauseMenu != null && questMenu.activeInHierarchy) {
				pauseMenu.SetActive(false);
			}
			else {
				pauseMenu.SetActive(true);
			}
		}
		else {
			Debug.Log("Cannot toggle quest menu, gameMode is not game.");
		}
	}
}
