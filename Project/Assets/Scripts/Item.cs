using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class Item : MonoBehaviour {

    [SerializeField]
    private UnityEvent m_onPickup;

    [SerializeField]
    private UnityEvent m_onDrop;

    private Player m_owner;

    [SerializeField]
    private Transform m_handle;

    public Transform Handle
    {
        get { return m_handle;  }        
    }

    private ContextAction m_action;

    // Use this for initialization
    void Start() {
        m_action = GetComponent<ContextAction>();

        m_action.TriggerFilter = Filter;
        m_action.OnTrigger = Trigger;
        m_action.Reset();
    }

    public bool Filter(Player player)
    {
        return m_owner == null || m_owner == player;
    }

    public void Trigger(Player player, ContextAction.TriggerPhase phase)
    {
        if ((m_owner == player) && m_owner.CurrentItem == this)
        {
            player.CurrentItem = null;
        }
        else if (m_owner == null && player.CurrentItem == null)
        {
            player.CurrentItem = this;
        }

        m_action.Reset();

        if (m_action.CurrentPhase == ContextAction.TriggerPhase.Ended)
        {
            Debug.LogAssertionFormat("Item in invalid state! {0}", m_action.CurrentPhase);
        }      
    }

    public void Pickup(Player player)
    {
        GetComponent<Collider>().enabled = true;
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }        
        if (GetComponent<NavMeshAgent>())
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }
        m_owner = player;        

        m_onPickup.Invoke();
    }

    public void Drop(Player player)
    {
        GetComponent<Collider>().enabled = true;
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().velocity = Vector3.up * 2.5f;
        }
        if (GetComponent<NavMeshAgent>())
        {
            GetComponent<NavMeshAgent>().enabled = true;
        }
        m_owner = null;

        m_onDrop.Invoke();
    }
}
