using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScheduleEvent {
    public string name;
    [Header("Conditions")]
    public GameTimestamp time;
    public GameTimestamp.DayOfTheWeek dayOfTheWeek;

    public int priority;
    public bool ignoreDayOfTheWeek;
    public bool factorDate;

    [Header("Position")]
    public Locations location;
    public Vector3 facing;

    public enum Locations
    {
        Locker,
        BeforeClass,
        Seat,
        Lunch,
        AfterClass,
        Leave
    }
}
