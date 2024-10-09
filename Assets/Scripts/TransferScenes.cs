
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferScenes : MonoBehaviour {
	[SerializeField] string sceneName;
	[SerializeField] GameObject player;

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Scene sceneToLoad = SceneManager.GetSceneByName(sceneName);
			SceneManager.LoadScene(sceneName);
			SceneManager.MoveGameObjectToScene(player.gameObject, sceneToLoad);
		}
	}
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
}
