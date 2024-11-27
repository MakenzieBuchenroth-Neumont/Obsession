using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour {
	[SerializeField] private List<Student> students;
	[SerializeField] private GameObject studentImagePrefab;
	[SerializeField] private Transform contentArea;
	// Start is called before the first frame update

	private void Start() {
		populateScrollView();
	}

	private void populateScrollView() {
		foreach (Transform child in contentArea) {
			Destroy(child.gameObject);
		}

		foreach (Student student in students) {
			if (student == null || student.StudentImage == null || student.StudentName == null) {
				continue;
			}

			GameObject newPanel = Instantiate(studentImagePrefab, contentArea);

			Image imageComponent = newPanel.GetComponentInChildren<Image>();
			if (imageComponent != null) {
				imageComponent.sprite = student.StudentImage;
			}

			TMP_Text nameText = newPanel.GetComponentInChildren<TMP_Text>();
			if (nameText != null) {
				nameText.text = student.StudentName;
			}
		}
	}
}
