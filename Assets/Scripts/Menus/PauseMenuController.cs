using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {
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
			pauseMenu.SetActive(false);
		}
		else {
			pauseMenu.SetActive(true);
		}
	}

	void Update() {
		if (allowedScenes.Contains(SceneManager.GetActiveScene().name)) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				pauseMenu.SetActive(!pauseMenu.activeSelf);
			}
		}
	}
}
