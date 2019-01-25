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
    private Text m_dayText;

    [SerializeField]
    private Transform m_trashPrefab;

    private List<TaskBroadcaster> m_broadcasters = new List<TaskBroadcaster>();

    private Dictionary<ScheduleEvent, int> m_eventOccurances = new Dictionary<ScheduleEvent, int>();

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

    }

    // Update is called once per frame
    void Update()
    {
        Schedule schedule = m_days[m_currentDay];

        m_dayText.text = string.Format("DAY {0}", m_currentDay);

        float m_prevTime = m_currentTime;

        m_currentTime += Time.deltaTime;

        int prevMinute = (int)Mathf.Floor(m_prevTime / schedule.Envronment.DayLength * 24.0f * 12.0f);
        int minute = (int)Mathf.Floor(m_currentTime / schedule.Envronment.DayLength * 24.0f * 12.0f);
        float normTime = m_currentTime / schedule.Envronment.DayLength * 24.0f;        

        float unitTime = m_currentTime / schedule.Envronment.DayLength;

        m_sun.intensity = schedule.Envronment.SunIntensity.Evaluate(unitTime);
        m_sun.color = schedule.Envronment.SunColor.Evaluate(unitTime);

        if (prevMinute != minute)
        {
            // Minute tick has occurred, evaluate events

            foreach (ScheduleEvent e in schedule.Events)
            {
                if (normTime >= e.TimeOfDayStart && normTime <= e.TimeOfDayEnd)
                {
                    int count = 0;
                    if (!m_eventOccurances.TryGetValue(e, out count))
                    {
                        m_eventOccurances[e] = 0;
                    }

                    if (count < e.TimesMax && Random.value < e.InstanceChance)
                    {
                        m_eventOccurances[e]++;
                        SpawnEvent(e);
                    }
                }
            }

        }

        if (m_currentTime >= schedule.Envronment.DayLength)
        {
            if (m_days.Length > m_currentDay + 1)
            {
                // TODO: Handle end of day
            }
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
