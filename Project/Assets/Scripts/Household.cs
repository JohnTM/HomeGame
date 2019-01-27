using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Household : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_onScheduleComplete = new UnityEvent();

    public UnityEvent OnScheduleComplete
    {
        get
        {
            return m_onScheduleComplete;
        }
    }


    [SerializeField]
    private Schedule[] m_days;

    [SerializeField]
    private int m_currentDay;
    private float m_currentTime;

    [SerializeField]
    private Light m_sun;

    [SerializeField]
    private TaskListUI m_taskListUI;

    [SerializeField]
    private EmotionalStateUI m_emotionalStateUI;

    public EmotionalStateUI EmotionalState
    {
        get
        {
            return m_emotionalStateUI;
        }
    }



    [SerializeField]
    private Text m_dayText;

    [SerializeField]
    private Slider m_daySlider;

    [SerializeField]
    private Transform m_trashPrefab;

    [SerializeField]
    private Transform m_laundryPrefab;

    [SerializeField]
    private Transform m_babyPrefab;

    public bool Paused
    {
        get { return Time.timeScale == 0.0f; }
        set
        {
            Time.timeScale = value ? 0.0f : 1.0f;
            m_emotionalStateUI.GetComponentInParent<Canvas>().enabled = !value;
        }
    }

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

        /*if (!SceneManager.GetSceneByName("Menus").isLoaded)
        {
            SceneManager.LoadScene("Menus", LoadSceneMode.Additive);
        }       */ 
    }

    public void Punish(int amount, TaskBroadcaster source)
    {
        m_taskListUI.SpawnPunishTick(source, m_emotionalStateUI);        
    }

    public void Reward(int amount, TaskBroadcaster source)
    {
        m_taskListUI.SpawnRewardTick(amount, source, m_emotionalStateUI);
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

        }

        if (m_currentTime >= schedule.Envronment.DayLength)
        {
            if (m_days.Length > m_currentDay + 1)
            {
                // TODO: Handle end of day
                m_currentDay++;
                SetupSchedule(m_days[m_currentDay]);
                m_currentTime = 0;
            }        
            else
            {
                // Finished!
                m_onScheduleComplete.Invoke();
            }
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
            case ScheduleEventType.AddBaby: AddBaby(); break;
            case ScheduleEventType.GrowLawn: GrowLawn(); break;
            case ScheduleEventType.RandomLaundry: RandomLaundry(); break;
        }
    }    

    public int NavMeshMask(params string[] names)
    {
        int mask = 0;
        foreach (string n in names)
        {
            mask |= (1 << NavMesh.GetAreaFromName(n));
        }

        return mask;
    }

    Vector3 GetRandomLocation(int mask = 0)
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int[] areas = navMeshData.areas;

        int t = Random.Range(0, navMeshData.indices.Length/3);
        if (mask != 0)
        {
            while (((1 << areas[t]) & mask) == 0)
            {
                t = Random.Range(0, navMeshData.indices.Length/3);
            }
        }

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t*3]], navMeshData.vertices[navMeshData.indices[t*3 + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t*3 + 2]], Random.value);

        return point;
    }

    public void DirtyDishes()
    {
        FindObjectOfType<Appliance>().Reset();
    }

    public void RandomTrash()
    {
        Vector3 pos = GetRandomLocation(NavMeshMask("IndoorsCarpet", "IndoorsWood")) + Vector3.up * 0.5f;
        Instantiate<Transform>(m_trashPrefab, pos, Quaternion.identity);
    }

    public void RandomLaundry()
    {
        Vector3 pos = GetRandomLocation(NavMeshMask("IndoorsCarpet")) + Vector3.up * 0.5f;
        Instantiate<Transform>(m_laundryPrefab, pos, Quaternion.identity);
    }

    public void AddBaby()
    {
        Vector3 pos = GetRandomLocation(NavMeshMask("IndoorsCarpet")) + Vector3.up * 0.5f;
        Transform baby = Instantiate<Transform>(m_babyPrefab, pos, Quaternion.identity);
    }

    public void GrowLawn()
    {
        List<Grass> results = new List<Grass>();
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().ToList().ForEach(g => results.AddRange(g.GetComponentsInChildren<Grass>(true)));
        foreach (Grass g in results)
        {
            g.enabled = true;
        }
    }
}
