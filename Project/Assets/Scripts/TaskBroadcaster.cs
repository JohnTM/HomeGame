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
    public float MaxDuration;
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

    private Household m_household;

    public void Activate()
    {
        m_active = true;
        m_household.AddBroadcaster(this);

        var renderer = GetComponentInChildren<Renderer>();
        if (renderer)
        {
            renderer.material.SetColor("_EmissionColor", new Color(1.0f, 1.0f, 0.0f) * 0.5f);
            renderer.material.EnableKeyword("_EMISSION");
        }
    }

    public void Completed()
    {
        if (m_active)
        {
            m_active = false;
            m_household.RemoveBroadcaster(this);

            var renderer = GetComponentInChildren<Renderer>();
            if (renderer)
            {
                renderer.material.SetColor("_EmissionColor", new Color(0.0f, 0.0f, 0.0f));
                renderer.material.EnableKeyword("_EMISSION");
            }
        }
    }

	// Use this for initialization
	void Start ()
    {
        m_household = FindObjectOfType<Household>();

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
		
	}
}
