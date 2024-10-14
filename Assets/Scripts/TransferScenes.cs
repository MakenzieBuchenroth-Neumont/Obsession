
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferScenes : MonoBehaviour {
	[SerializeField] string sceneName;
	[SerializeField] GameObject player;

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			StartCoroutine(LoadSceneAsync());
		}
	}

	private IEnumerator LoadSceneAsync() {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

		while (!asyncLoad.isDone) {
			yield return null;
		}
		Scene sceneToLoad = SceneManager.GetSceneByName(sceneName);
		SceneManager.MoveGameObjectToScene(player, sceneToLoad);
	}
}
