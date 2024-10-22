using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEUIManager : MonoBehaviour {
	public Image buttonImage;
	public Image progressCircle;
	private float qteDuration = 3f;
	private Coroutine qteCoroutine;

	public void startQTE(KeyCode key) {
		buttonImage.gameObject.SetActive(true);
		progressCircle.gameObject.SetActive(true);
		progressCircle.fillAmount = 1f;

		if (qteCoroutine != null) {
			StopCoroutine(qteCoroutine);
		}
		qteCoroutine = StartCoroutine(QTECoroutine());
	}

	private IEnumerator QTECoroutine() {
		float timeleft = qteDuration;

		while (timeleft > 0) {
			timeleft -= Time.deltaTime;
			progressCircle.fillAmount = timeleft / qteDuration;
			yield return null;
		}
		EndQTE(false);
	}

	public void EndQTE(bool success) {
		buttonImage.gameObject.SetActive(false);
		progressCircle.gameObject.SetActive(false);
		progressCircle.fillAmount = 0f;

		var interactable = FindObjectOfType<NPCInteractable>();
		if (success) {
			interactable.handleQTESuccess();
		}
		else {
			interactable.handleQTEFailure();
		}
	}
}
