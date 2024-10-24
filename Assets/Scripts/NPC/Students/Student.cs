using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Character/New Student")]
public class Student : ScriptableObject {
	[Header("Basic Information")]
	[SerializeField] private string studentName;
	[SerializeField] private studentYear year;
	[SerializeField] private studentPersonality personality;
	[SerializeField] private StudentClassType studentClass;
	[SerializeField] private Sprite studentImage;
	[SerializeField] public GameObject prefab;
	[Header("Schedules")]
	[SerializeField] public NPCScheduleData myScheduleData;
	[SerializeField] public Vector3 spawnPoint;
	[SerializeField] public GameObject Locker;
	[SerializeField] public GameObject Seat;

	[Header("Dialogue and Quest")]
	public Quest quest;

	[SerializeField] private string[] dialogueLines;
	[SerializeField] private string[] talkLines;
	[SerializeField] private string[] askLines;
	[SerializeField] private string[] questLines;
	[SerializeField] private string[] goodbyeLines;

	[Header("World Information")]
	private bool isDead = false;
	public bool IsDead {
		get { return isDead; }
		set {
			isDead = value;
		}
	}
	public bool isMoving = false;

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

	public Vector3 facingDir = Vector3.zero;

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

	public void reactToWeapon() {
		if (IsDead) {
			Debug.Log($"{name} cannot react because they are dead.");
		};

		Debug.Log($"{name} saw you carrying a weapon!");

		Animator anim = prefab.GetComponent<Animator>();
		if (anim != null) {
			anim.SetTrigger("WeaponReact");
		}

		AudioSource audio = prefab.GetComponent<AudioSource>();
		if (audio != null) {
			audio.Play();
		}
	}

	public Vector3 getTargetPosition(ScheduleEvent.Locations locationEnum) {
        switch (locationEnum)
        {
            default:
            case ScheduleEvent.Locations.Locker:
                return Locker.transform.position;
            case ScheduleEvent.Locations.Seat:
                return Seat.transform.position;
        }
    }
}