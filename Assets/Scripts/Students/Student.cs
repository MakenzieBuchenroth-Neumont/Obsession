using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Character/New Student")]
public class Student : ScriptableObject {
	[SerializeField] string studentName;
	[SerializeField] studentYear year = new studentYear();
	[SerializeField] studentPersonality personality = new studentPersonality();
	[SerializeField] studentClass Class = new studentClass();
	[SerializeField] Sprite studentImage;
	[SerializeField] GameObject prefab;

	enum studentYear {
		One = 1,
		Two = 2,
		Three = 3,
		Four = 4
	}

	enum studentPersonality {
		Loner,
		TeachersPet,
		Heroic,
		Coward,
		Evil,
		SocialButterfly,
		Tsundere,
		Scrict,
		Violent,
		Sleuth,
		Lovestruck,
		Yandere
	}

	enum studentClass {
		OneA,
		OneB,
		OneC,
		OneD,
		TwoA,
		TwoB,
		TwoC,
		TwoD,
		ThreeA,
		ThreeB,
		ThreeC,
		ThreeD,
		FourA,
		FourB,
		FourC,
		FourD
	}
}