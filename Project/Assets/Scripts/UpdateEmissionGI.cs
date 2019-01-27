using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UpdateEmissionGI : MonoBehaviour {

    Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > 10f)
        {
            RendererExtensions.UpdateGIMaterials(rend);
        }
	}
}
