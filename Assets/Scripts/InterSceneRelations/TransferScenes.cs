
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
		if (other.gameObject.tag == "NPC") {
			NPCManager npcManager = other.GetComponent<NPCManager>();
			if (npcManager != null) {
				StartCoroutine(moveNPCToScene(other.gameObject));
			}
		}
	}

	private IEnumerator moveNPCToScene(GameObject npcInstance) {
		Scene targetScene = SceneManager.GetSceneByName(sceneName);

		if (!targetScene.isLoaded) {
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			while (!asyncLoad.isDone) {
				yield return null;
			}
		}
		SceneManager.MoveGameObjectToScene(npcInstance, SceneManager.GetSceneByName(sceneName));
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
