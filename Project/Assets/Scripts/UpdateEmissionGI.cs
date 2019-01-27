using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UpdateEmissionGI : MonoBehaviour {

    Renderer rend;

    float startTime;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > startTime + 5f)
        {
            RendererExtensions.UpdateGIMaterials(rend);
        }
	}
}
