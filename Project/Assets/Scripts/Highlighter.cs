using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(cakeslice.Outline))]
public class Highlighter : MonoBehaviour {

    public bool Show
    {
        get
        {
            return m_show;
        }

        set
        {
            m_show = value;

            foreach (var o in m_outlines)
            {
                o.enabled = m_show;
            }
        }
    }

    private bool m_show;

    private cakeslice.Outline[] m_outlines;

	// Use this for initialization
	void Start () {
        m_outlines = GetComponentsInChildren<cakeslice.Outline>();
        Show = false;
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
