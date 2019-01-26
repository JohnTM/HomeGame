using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WashingMachine : MonoBehaviour {

    public void On()
    {
        transform.DOShakePosition(1.0f, 0.1f, 20, 90, false, false).SetLoops(-1);
    }

    public void Off()
    {
        transform.DOKill();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
