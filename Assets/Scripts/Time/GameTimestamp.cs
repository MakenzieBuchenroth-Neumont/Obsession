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

	public int day;
	public int hour;
	public int minute;

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

		if (hour == 18) {
			SceneManager.LoadScene("Bedroom");
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