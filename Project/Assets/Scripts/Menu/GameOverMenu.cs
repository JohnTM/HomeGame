using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverMenu : MonoBehaviour {

    [SerializeField]
    private Transform m_depressionCamera;

    [SerializeField]
    private Text m_depressionText;

    public TaskBroadcaster Source
    {
        get
        {
            return m_source;
        }

        set
        {
            m_source = value;
            m_depressionText.text = m_source.Task.Name;
        }
    }

    private TaskBroadcaster m_source;

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        m_depressionCamera.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
		if (m_source)
        {
            m_depressionCamera.position = m_source.transform.position - m_depressionCamera.forward * 2.0f + Random.insideUnitSphere * 0.01f;
        }
    }
}
