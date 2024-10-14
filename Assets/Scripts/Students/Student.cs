using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Character/New Student")]
public class Student : ScriptableObject {
	[SerializeField] private string studentName;
	[SerializeField] private studentYear year;
	[SerializeField] private studentPersonality personality;
	[SerializeField] private StudentClassType studentClass;
	[SerializeField] private Sprite studentImage;
	[SerializeField] public GameObject prefab;
	[SerializeField] public Vector3 spawnPoint;

	[SerializeField] private string[] dialogueLines;
	[SerializeField] private string[] talkLines;
	[SerializeField] private string[] askLines;
	[SerializeField] private string[] questLines;
	[SerializeField] private string[] goodbyeLines;

	public string StudentName => studentName;
	public studentYear Year => year;
	public studentPersonality Personality => personality;
	public StudentClassType StudentClass => studentClass;
	public Sprite StudentImage => studentImage;
	public string[] DialogueLines => dialogueLines;
	public string[] TalkLines => talkLines;
	public string[] AskLines => askLines;
	public string[] QuestLines => questLines;
	public string[] GoodbyeLines => goodbyeLines;

	public enum studentYear {
		One = 1,
		Two = 2,
		Three = 3,
		Four = 4
	}

	public enum studentPersonality {
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

	public enum StudentClassType {
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