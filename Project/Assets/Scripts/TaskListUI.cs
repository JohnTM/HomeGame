using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListUI : MonoBehaviour {

    [SerializeField]
    private TaskIcon m_taskIconPrefab;

    private Dictionary<TaskBroadcaster, TaskIcon> m_taskIconMap = new Dictionary<TaskBroadcaster, TaskIcon>();

    public void BroadcasterAdded(TaskBroadcaster tb)
    {
        if (!m_taskIconMap.ContainsKey(tb))
        {
            TaskIcon icon = Instantiate<TaskIcon>(m_taskIconPrefab);
            icon.Text.text = tb.Task.Name;
            icon.transform.SetParent(transform);
            m_taskIconMap.Add(tb, icon);
        }
    }

    public void BroadcasterRemoved(TaskBroadcaster tb)
    {
        if (m_taskIconMap.ContainsKey(tb))
        {
            TaskIcon icon = m_taskIconMap[tb];            
            m_taskIconMap.Remove(tb);
            Destroy(icon.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
