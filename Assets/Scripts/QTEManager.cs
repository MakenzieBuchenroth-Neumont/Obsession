using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour {
	public static QTEManager Instance { get; private set; }

	[Header("QTE UI Elements")]
	public GameObject qtePanel;
	public Image qteButton;
	public Image progressBar;

	private Coroutine qteCoroutine;

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		}
		else {
			Instance = this;
		}
	}

	public void StartQTE(KeyCode button, float duration, NPCInteractable interactable) {
		if (qteCoroutine != null) {
			StopCoroutine(qteCoroutine);
		}
		qteCoroutine = StartCoroutine(QTECoroutine(button, duration, interactable));
	}

	private IEnumerator QTECoroutine(KeyCode keyCode, float duration, NPCInteractable interactable) {
		qtePanel.SetActive(true);
		float time = 0;

		while (time < duration) {
			time += Time.deltaTime;
			progressBar.fillAmount = time / duration;

            if (Input.GetKeyDown((keyCode))) {
				EndQTE(true, interactable);
				yield break;
			}
			yield return null;
        }
		EndQTE(false, interactable);
	}

	private void EndQTE(bool success, NPCInteractable interactable) {
		qtePanel.SetActive(false);
		progressBar.fillAmount = 0;

		if (success) {
			if (interactable != null) {
				interactable.handleQTESuccess();
			}
		}
		else {
			if (interactable != null) {
				interactable.handleQTEFailure();
			}
		}
	}
}
