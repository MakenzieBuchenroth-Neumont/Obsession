using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour {
	[SerializeField] GameObject player;
	[SerializeField] GameObject manager;
	[SerializeField] GameObject canvas;

	public void quitButton() {
		GameManager.gameMode = GameManager.GameMode.Title;
		GameObject.Destroy(player);
		GameObject.Destroy(manager);
		GameObject.Destroy(canvas);
		SceneManager.LoadScene("Title");
	}

	public void quitToDesktop() {
		Application.Quit();
	}
}
