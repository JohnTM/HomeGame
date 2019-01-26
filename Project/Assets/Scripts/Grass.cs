using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour {

    [SerializeField]
    private Transform m_blades;

    [SerializeField]
    private float m_minHeightScale = 0.1f;

    [SerializeField]
    private float m_maxHeightScale = 4.0f;

    [SerializeField]
    private float m_growthTime = 90;

    private float m_timer = 0;

    private TaskBroadcaster m_tb;

	// Use this for initialization
	void Start () {
        m_tb = GetComponent<TaskBroadcaster>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_timer += Time.deltaTime * Random.Range(0.8f, 1.2f);
        if (m_timer > m_growthTime && m_tb.Active == false)
        {
            m_timer = m_growthTime;
            m_tb.Activate();
        }

        float growth = m_timer / m_growthTime;
        m_blades.localScale = new Vector3(1, Mathf.Lerp(m_minHeightScale, m_maxHeightScale, growth), 1);
	}

    public void Mow()
    {
        m_timer = 0;

        if (m_tb.Active)
        {            
            m_tb.Completed();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Mower>())
        {
            Mow();
        }
    }
}
