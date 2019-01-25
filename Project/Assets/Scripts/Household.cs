using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Household : MonoBehaviour
{
    [SerializeField]
    private TaskListUI m_taskListUI;

    private List<TaskBroadcaster> m_broadcasters = new List<TaskBroadcaster>();

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
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    	
	}
}
