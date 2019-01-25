using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appliance : MonoBehaviour {

    void Start()
    {
        ContextAction action = GetComponent<ContextAction>();

        action.TriggerFilter = Filter;
        action.OnTrigger = Trigger;

        action.Reset(); 
        GetComponent<TaskBroadcaster>().Activate();
    }

    public bool Filter(Player player)
    {
        return (player.CurrentItem == null);
    }

    public void Trigger(Player player, ContextAction.TriggerPhase phase)
    {
        if (phase == ContextAction.TriggerPhase.Ended)
        {
            GetComponent<TaskBroadcaster>().Completed();
        }
    }
}
