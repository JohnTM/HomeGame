using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    private Player m_owner;

    // Use this for initialization
    void Start() {
        ContextAction action = GetComponent<ContextAction>();

        action.TriggerFilter = Filter;
        action.OnTrigger = Trigger;
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
            GetComponent<ContextAction>().Reset();
        }
        else if (m_owner == null && player.CurrentItem == null)
        {
            player.CurrentItem = this;
            GetComponent<ContextAction>().Reset();
        }
    }

    public void Pickup(Player player)
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;
        m_owner = player;
    }

    public void Drop(Player player)
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = Vector3.up * 2.5f;
        m_owner = null;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
