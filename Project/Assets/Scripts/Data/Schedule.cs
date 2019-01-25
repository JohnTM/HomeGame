using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScheduleEventType
{
    RandomTrash,
    RandomLaundry,
    RandomToy,
    DustyFloor,
    DirtyCarpet,
    AddBaby,
    DirtyDishes,
    GrowLawn,
    MailDelivery,
    PaperDelivery,
    NeedToilet,
    NeedShower,
    NeedBreakfast,
    NeedLunch,
    NeedDinner
}

[System.Serializable]
public class ScheduleEvent
{
    public ScheduleEventType Type;
    [Range(0, 24)]
    public float TimeOfDayStart = 1;
    [Range(0, 24)]
    public float TimeOfDayEnd = 23;
    [Range(0, 1)]
    public float EventChance = 1;
    [Range(0, 1)]
    public float InstanceChance = 1;
    [Range(0,30)]
    public int TimesMin = 1;
    [Range(0, 30)]
    public int TimesMax = 1;
}

[CreateAssetMenu(fileName = "Data", menuName = "Household/Schedule", order = 1)]
public class Schedule : ScriptableObject
{
    public Environment Envronment;
    public List<ScheduleEvent> Events = new List<ScheduleEvent>();
}


