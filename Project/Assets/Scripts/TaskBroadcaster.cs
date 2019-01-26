using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TaskPriority
{
    Low,
    Medium,
    High
}

[System.Serializable]
public class Task
{
    public string Name;
    public Sprite Icon;
    public TaskPriority Priority;
    public bool Grouped;
    public int GroupThreshold;
    public float PunishAmount = 1;
    public float PunishRate = 0.5f;
    public float PunishTime = 30.0f;
    public float PunishMultiplier = 2.0f;
    public int Reward = 5;
}

public class TaskBroadcaster : MonoBehaviour
{
    [SerializeField]
    private bool m_activateOnLoad = false;

    private bool m_active;

    [SerializeField]
    private Task m_task;

    public Task Task
    {
        get
        {
            return m_task;
        }
    }

    public bool Active
    {
        get { return m_active; }
    }


    private Household m_household;
    private Highlighter m_highlighter;

    public float TotalTime
    {
        get { return m_totalTime; }
    }

    private float m_totalTime;
    private float m_punishTimer;

    public void Activate()
    {
        m_active = true;
        m_totalTime = 0;
        m_punishTimer = 0;
        m_household.AddBroadcaster(this);

        if (m_highlighter)
        {
            m_highlighter.Show = true;
        }
    }

    public void Completed()
    {
        if (m_active)
        {
            m_active = false;
            m_household.Reward(m_task.Reward, this);
            m_household.RemoveBroadcaster(this);            

            if (m_highlighter)
            {
                m_highlighter.Show = false;
            }
        }
    }

    private void Awake()
    {
        m_household = FindObjectOfType<Household>();
        m_highlighter = GetComponent<Highlighter>();
    }

    // Use this for initialization
    void Start ()
    {
        if (m_activateOnLoad)
        {
            Activate();
        }
    }

    void OnDestroy()
    {
        Completed();
    }

    // Update is called once per frame
    void Update ()
    {
		if (m_active)
        {
            m_totalTime += Time.deltaTime;
            m_punishTimer += Time.deltaTime;

            float rate = m_totalTime < m_task.PunishTime ? m_task.PunishRate : m_task.PunishRate * m_task.PunishMultiplier;

            if (m_punishTimer > 1.0f / rate)
            {
                m_punishTimer = 0.0f;
                m_household.Punish(1, this);
            }
        }
	}
}
