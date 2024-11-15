using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]

public class GameTimestamp {

	public enum DayOfTheWeek {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
		Saturday
	}

	public enum ScheduleBlock {
		BeforeClass,
		Class,
		Lunch,
		AfterClass,
		Evening
	}

	public int day;
	public int hour;
	public int minute;

	[SerializeField] public GameObject playerTransform;
	[SerializeField] public GameObject mainCamera;
	[SerializeField] public GameObject bedroomCamera;

	public ScheduleBlock scheduleBlock;

	//Constructor to set up the class
	public GameTimestamp(int day, int hour, int minute) {
		this.day = day;
		this.hour = hour;
		this.minute = minute;
	}

	//Makes an increment of the time by 1 minute
	public void UpdateClock() {
		minute++;

		//60 minutes in 1 hour
		if (minute >= 60) {
			//reset minutes
			minute = 0;
			hour++;
		}

		//24 hours in 1 day
		if (hour >= 24) {
			//Reset hours 
			hour = 0;
			day++;
		}

		/* 0 = midnight
		// 1 = 1
		// 2 = 2
		// 3 = 3
		// 4 = 4
		// 5 = 5
		// 6 = 6
		// 7 = 7
		// 8 = 8
		// 9 = 9
		// 10 = 10
		// 11 = 11
		// 12 = 12
		// 13 = 1
		// 14 = 2
		// 15 = 3
		// 16 = 4
		// 17 = 5
		// 18 = 6
		// 19 = 7
		// 20 = 8
		// 21 = 9
		// 22 = 10
		 23 = 11*/


		/* midnight - 7:59: before class
		// 8 - 11:59: class
		// 12-12.59: lunch
		// 1 - 3: class
		// 3 - 5:59: after class
		// 6 - 11:59: evening*/
		
		if (hour > 0 && (hour == 7 && minute == 59)) {
			scheduleBlock = ScheduleBlock.BeforeClass;
		}
		else if (hour > 8 || hour < 12) {
			scheduleBlock = ScheduleBlock.Class;
		}
		else if ((hour == 12 && minute >= 59)) {
			scheduleBlock = ScheduleBlock.Lunch;
		}
		else if (hour > 1 && hour < 3) {
			scheduleBlock = ScheduleBlock.Class;
		}
		else if (hour > 3 && hour < 6) {
			scheduleBlock = ScheduleBlock.AfterClass;
		}
		else {
			scheduleBlock = ScheduleBlock.Evening;
		}

		if (hour == 18) {
			playerTransform.transform.position = new Vector3(161.856f, 0, .914f);
			playerTransform.transform.rotation = new Quaternion(0, -90, 0, 0);
			mainCamera.SetActive(false);
			bedroomCamera.SetActive(true);

			TimeManager.Instance.timeScale = 0f;
		}
	}

	public DayOfTheWeek GetDayOfTheWeek() {
		//Convert the total time passed into days
		int daysPassed = day;

		//Remainder after dividing daysPassed by 7
		int dayIndex = daysPassed % 7;

		//Cast into Day of the Week
		return (DayOfTheWeek)dayIndex;
	}

	//Convert hours to minutes
	public static int hoursToMinutes(int hour) {
		//60 minutes = 1 hour
		return hour * 60;
	}

	//Convert Days to Hours
	public static int daysToHours(int days) {
		//24 Hours in a day
		return days * 24;
	}

	//Years to Days
	public static int yearsToDays(int years) {
		return years * 4 * 30;
	}

	public static bool Compare(GameTimestamp a, GameTimestamp b) {
		int timea = (a.hour * 60) + a.minute;
		int timeb = (b.hour * 60) + b.minute;

		return (timea <= timeb);
	}
}