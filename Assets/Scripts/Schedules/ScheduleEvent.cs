using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
    public Vector3 coord;
    public Vector3 facing;
}
