using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Household : MonoBehaviour
{
    [SerializeField]
    private Schedule[] m_days;

    private int m_currentDay;
    private float m_currentTime;

    [SerializeField]
    private Light m_sun;

    [SerializeField]
    private TaskListUI m_taskListUI;

    [SerializeField]
    private EmotionalStateUI m_emotionalStateUI;

    [SerializeField]
    private Text m_dayText;

    [SerializeField]
    private Slider m_daySlider;

    [SerializeField]
    private Transform m_trashPrefab;

    private List<TaskBroadcaster> m_broadcasters = new List<TaskBroadcaster>();

    private class EventDetail
    {
        public List<int> Ticks = new List<int>();
    }

    private Dictionary<ScheduleEvent, EventDetail> m_eventDetails = new Dictionary<ScheduleEvent, EventDetail>();

    public void AddBroadcaster(TaskBroadcaster tb)
    {
        if (!m_broadcasters.Contains(tb))
        {
            m_broadcasters.Add(tb);
            m_taskListUI.BroadcasterAdded(tb);
        }
    }

    public void RemoveBroadcaster(TaskBroadcaster tb)
    {
        if (m_broadcasters.Contains(tb))
        {
            m_broadcasters.Remove(tb);
            m_taskListUI.BroadcasterRemoved(tb);
        }
    }

    // Use this for initialization
    void Start()
    {
        SetupSchedule(m_days[m_currentDay]);
    }

    public void Punish(int amount, TaskBroadcaster source)
    {
        m_taskListUI.SpawnPunishTick(source, m_emotionalStateUI);        
    }

    void Reward()
    {

    }

    void SetupSchedule(Schedule schedule)
    {
        m_eventDetails.Clear();
        foreach (ScheduleEvent e in schedule.Events)
        {
            if (Random.value < e.EventChance)
            {
                EventDetail detail = new EventDetail();
                m_eventDetails[e] = detail;

                int count = Random.Range(e.TimesMin, e.TimesMax);

                float startTime = e.TimeOfDayStart / 24.0f * schedule.Envronment.DayLength;
                float endTime = e.TimeOfDayEnd / 24.0f * schedule.Envronment.DayLength;

                for (int i = 0; i < count; i++)
                {
                    if (Random.value < e.InstanceChance)
                    {
                        float timeA = startTime + ((float)i / count) * (endTime - startTime);
                        float timeB = startTime + ((float)(i+1) / count) * (endTime - startTime);
                        detail.Ticks.Add(TickForTime(Mathf.Lerp(timeA, timeB, Random.value)));
                    }                    
                }
            }
            
            
        }
    }

    int TickForTime(float time)
    {
        Schedule schedule = m_days[m_currentDay];
        return (int)Mathf.Floor(time / schedule.Envronment.DayLength * 24.0f * 12.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Schedule schedule = m_days[m_currentDay];

        m_dayText.text = string.Format("DAY {0}", m_currentDay+1);

        float prevTime = m_currentTime;

        m_currentTime += Time.deltaTime;

        int prevTick = TickForTime(prevTime);
        int tick = TickForTime(m_currentTime);
        float normTime = m_currentTime / schedule.Envronment.DayLength * 24.0f;        

        float unitTime = m_currentTime / schedule.Envronment.DayLength;

        m_sun.intensity = schedule.Envronment.SunIntensity.Evaluate(unitTime);
        m_sun.color = schedule.Envronment.SunColor.Evaluate(unitTime);

        m_daySlider.value = unitTime;

        if (prevTick != tick)
        {
            // Minute tick has occurred, evaluate events

            foreach(var pair in m_eventDetails)
            {
                foreach(int eventTick in pair.Value.Ticks)
                {
                    if (eventTick == tick)
                    {
                        SpawnEvent(pair.Key);
                    }
                }
            }


            //foreach (ScheduleEvent e in schedule.Events)
            //{
            //    if (normTime >= e.TimeOfDayStart && normTime <= e.TimeOfDayEnd)
            //    {
            //        int count = 0;
            //        if (!m_eventOccurances.TryGetValue(e, out count))
            //        {
            //            m_eventOccurances[e] = 0;
            //        }

            //        if (count < e.TimesMax && Random.value < e.InstanceChance)
            //        {
            //            m_eventOccurances[e]++;
            //            SpawnEvent(e);
            //        }
            //    }
            //}

        }

        if (m_currentTime >= schedule.Envronment.DayLength)
        {
            //if (m_days.Length > m_currentDay + 1)
            //{
            //    // TODO: Handle end of day
            //}
            m_currentTime = schedule.Envronment.DayLength;
        }

        // TODO: Change sun light based on env settings

        // TODO: Spawn events

    }

    public void SpawnEvent(ScheduleEvent e)
    {
        switch (e.Type)
        {
            case ScheduleEventType.RandomTrash: RandomTrash(); break;
            case ScheduleEventType.DirtyDishes: DirtyDishes(); break;
        }
    }    

    Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }

    public void DirtyDishes()
    {
        FindObjectOfType<Appliance>().Reset();
    }

    public void RandomTrash()
    {
        Instantiate<Transform>(m_trashPrefab, GetRandomLocation() + Vector3.up * 0.5f, Quaternion.identity);
    }
}
