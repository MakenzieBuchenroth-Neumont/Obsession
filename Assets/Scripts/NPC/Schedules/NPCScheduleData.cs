using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/NPC Schedule")]
public class NPCScheduleData : ScriptableObject {
    public Student student;
    public List<ScheduleEvent> npcScheduleList;
}
