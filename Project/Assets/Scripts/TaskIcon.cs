using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskIcon : MonoBehaviour {

    [SerializeField]
    private Image m_image;

    [SerializeField]
    private Text m_text;

    public Text Text
    {
        get
        {
            return m_text;
        }
    }

    public Image Image
    {
        get
        {
            return m_image;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
