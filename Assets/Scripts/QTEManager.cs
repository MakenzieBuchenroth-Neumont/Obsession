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

	public void StartQTE(string button, float duration) {
		if (qteCoroutine != null) {
			StopCoroutine(qteCoroutine);
		}
		qteCoroutine = StartCoroutine(QTECoroutine(KeyCode.U, duration));
	}

	private IEnumerator QTECoroutine(KeyCode keyCode, float duration) {
		qtePanel.SetActive(true);
		float time = 0;

		while (time < duration) {
			time += Time.deltaTime;
			progressBar.fillAmount = time / duration;

            if (Input.GetKeyDown((keyCode))) {
				EndQTE(true);
				yield break;
			}
			yield return null;
        }
		EndQTE(false);
	}

	private void EndQTE(bool success) {
		qtePanel.SetActive(false);
		progressBar.fillAmount = 0;

			var interactable = FindObjectOfType<NPCInteractable>();
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
