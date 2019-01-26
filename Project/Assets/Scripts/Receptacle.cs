using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Receptacle : MonoBehaviour {

    [SerializeField]
    private UnityEvent m_onProcessingBegan;
    [SerializeField]
    private UnityEvent m_onProcessingEnded;

    [SerializeField]
    private string[] m_tags;

    [SerializeField]
    private int m_capacity = 5;

    [SerializeField]
    private float m_processingTime = 0;

    [SerializeField]
    private Item m_byproduct;

    private int m_usage;
    private bool m_pickupReady;

	// Use this for initialization
	void Start ()
    {
        ContextAction action = GetComponent<ContextAction>();

        action.TriggerFilter = Filter;
        action.OnTrigger = Trigger;
        action.Reset();
    }

    public bool Filter(Player player)
    {
        if (player.CurrentItem)
        {
            if (m_capacity > 0 && m_usage == m_capacity) return false;

            foreach (string tag in m_tags)
            {
                if (player.CurrentItem.tag == tag)
                {
                    return true;
                }
            }
        }
        else if (player.CurrentItem == null)
        {
            if (m_capacity > 0 && m_usage == m_capacity && GetComponent<TaskBroadcaster>().Active) return true;
        }

        return false;
    }

    public void Trigger(Player player, ContextAction.TriggerPhase phase)
    {
        if (player.CurrentItem == null)
        {
            if (m_capacity > 0 && m_usage == m_capacity)
            {
                if (m_byproduct)
                {
                    Item item = Instantiate<Item>(m_byproduct);
                    player.CurrentItem = item;
                    m_usage = 0;
                    GetComponent<TaskBroadcaster>().Completed();
                    GetComponent<ContextAction>().Reset();
                }
            }
        }
        else if (player.CurrentItem)
        {
            var item = player.CurrentItem;
            player.CurrentItem = null;
            Destroy(item.gameObject);
            GetComponent<ContextAction>().Reset();
            if (m_capacity > 0)
            {
                m_usage++;
                if (m_usage == m_capacity)
                {
                    if (m_processingTime > 0)
                    {
                        m_onProcessingBegan.Invoke();
                        DOTween.Sequence().AppendInterval(m_processingTime).AppendCallback(() =>
                        {
                            GetComponent<TaskBroadcaster>().Activate();
                            m_onProcessingEnded.Invoke();
                        });
                    }
                    else
                    {
                        GetComponent<TaskBroadcaster>().Activate();
                    }
                }
            }
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
