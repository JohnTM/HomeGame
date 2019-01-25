using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextAction : MonoBehaviour {

    public enum TriggerType
    {
        Instant,
        OverTime
    }

    public enum TriggerPhase
    {
        Idle,
        Began,
        InProgress,
        Ended
    }

    public float Progress
    {
        get
        {
            return Mathf.Clamp01(m_timer / m_duration);
        }
    }

    public TriggerPhase CurrentPhase
    {
        get
        {
            return m_currentPhase;
        }
    }


    [SerializeField]
    private TriggerType m_type = TriggerType.Instant;

    public TriggerType Type
    {
        get { return m_type; }    
    }

    [SerializeField]
    private float m_duration;

    private TriggerPhase m_currentPhase = TriggerPhase.Idle;
    private float m_timer;

    public delegate bool TriggerFilterDelegate(Player player);
    public delegate void TriggerDelegate(Player player, TriggerPhase phase);

    public TriggerFilterDelegate TriggerFilter;
    public TriggerDelegate OnTrigger;

    public bool CanTrigger(Player player)
    {
        if (TriggerFilter != null)
        {
            return TriggerFilter(player);
        }
        return false;
    }

    public void Reset()
    {
        m_currentPhase = TriggerPhase.Idle;
        m_timer = 0;
    }

    public TriggerPhase Trigger(Player owner)
    {
        bool fireEvent = false;

        switch (m_type)
        {
            case TriggerType.Instant:
                if (m_currentPhase == TriggerPhase.Idle)
                {
                    m_currentPhase = TriggerPhase.Ended;
                    fireEvent = true;
                }
                break;
            case TriggerType.OverTime:
                if (m_currentPhase == TriggerPhase.Idle)
                {
                    m_currentPhase = TriggerPhase.Began;
                    m_timer = 0;
                    fireEvent = true;
                }
                else if (m_currentPhase == TriggerPhase.Began || m_currentPhase == TriggerPhase.InProgress)
                {
                    m_currentPhase = TriggerPhase.InProgress;
                    m_timer += Time.deltaTime;
                    if (m_timer >= m_duration)
                    {
                        m_currentPhase = TriggerPhase.Ended;
                    }
                    fireEvent = true;
                }
                
                break;
        }


        if (fireEvent && OnTrigger != null)
        {
            OnTrigger(owner, m_currentPhase);
        }

        return m_currentPhase;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
