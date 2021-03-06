﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            if (m_outlines != null)
            {
                foreach (var o in m_outlines)
                {
                    o.enabled = m_show;
                }
            }
        }
    }

    private bool m_show;

    private Quick.Outline[] m_outlines;

    private void Awake()
    {

    }

    // Use this for initialization
    void Start () {
        //Show = false;
    }
	
	// Update is called once per frame
	void Update () {
        m_outlines = GetComponentsInChildren<Quick.Outline>();
        foreach (var o in m_outlines)
        {
            o.enabled = m_show;
        }
    }
}
