using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptacle : MonoBehaviour {

    [SerializeField]
    private string m_tag;

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
        return player.CurrentItem && player.CurrentItem.tag == m_tag;
    }

    public void Trigger(Player player, ContextAction.TriggerPhase phase)
    {
        var item = player.CurrentItem;
        player.CurrentItem = null;
        Destroy(item.gameObject);
        GetComponent<ContextAction>().Reset();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
