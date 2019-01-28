using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOverMenu : MonoBehaviour {

    [SerializeField]
    private Transform m_depressionCamera;

    [SerializeField]
    private Text m_depressionText;

    [SerializeField]
    private Text m_scoreText;

    public TaskBroadcaster Source
    {
        get
        {
            return m_source;
        }

        set
        {
            m_source = value;
            m_depressionText.text = m_source.Task.Name.ToUpper();
            m_depressionCamera.gameObject.SetActive(true);
            m_scoreText.text = string.Format("You lasted {0} days...", FindObjectOfType<Household>().CurrentDay + 1);
        }
    }

    private TaskBroadcaster m_source;

    // Update is called once per frame
    void Update () {
		if (m_source)
        {
            m_depressionCamera.position = m_source.transform.position - m_depressionCamera.forward * 2.0f + Random.insideUnitSphere * 0.01f;
        }
    }

    public void MenuPressed()
    {
        SceneManager.LoadScene(0);
    }
}
