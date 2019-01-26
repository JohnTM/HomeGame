using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Baby : MonoBehaviour {

    [SerializeField]
    private float m_wanderRadius = 1.0f;
    [SerializeField]
    private float m_wanderTime = 1.0f;
    [SerializeField]
    private float m_wanderTimeVariance = 0.25f;

    private float m_timer;

    private NavMeshAgent m_agent;

    private TaskBroadcaster m_tb;

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_tb = GetComponent<TaskBroadcaster>();
    }

    // Use this for initialization
    void Start () {
    }

    public void Cry()
    {
        m_tb.Activate();
    }
	
	// Update is called once per frame
	void Update () {

        if (m_agent.enabled)
        {
            m_timer -= Time.deltaTime;

            if (m_timer <= 0)
            {
                m_timer = m_wanderTime + Random.Range(-m_wanderTimeVariance, m_wanderTimeVariance);
                Vector2 dir = Random.insideUnitCircle;
                m_agent.destination = transform.position + new Vector3(dir.x, 0, dir.y) * m_wanderRadius;
            }
        }
        else if (m_tb.Active)
        {
            m_tb.Completed();
        }
    }
}
