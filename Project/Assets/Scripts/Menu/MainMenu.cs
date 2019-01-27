using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MainMenu : MonoBehaviour {

    [SerializeField]
    private Transform m_howToPlayMenu;

    [SerializeField]
    private Transform m_gameOverMenu;

    [SerializeField]
    private Transform m_playButton;

    [SerializeField]
    private Transform m_pointOfView;

    private CameraFollow m_cameraFollow;

    private Household m_household;

    private Transform m_currentMenu;

	// Use this for initialization
	void Start () {
        m_household = FindObjectOfType<Household>();
        m_cameraFollow = FindObjectOfType<CameraFollow>();
        m_cameraFollow.Override = m_pointOfView;
        m_household.Paused = true;
        GainFocus();
	}

    public void GainFocus()
    {
        EventSystem.current.SetSelectedGameObject(m_playButton.gameObject);
    }

    public void OpenMenu(Transform menu)
    {
        m_currentMenu = menu;
        m_currentMenu.gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        if (m_currentMenu)
        {
            m_currentMenu.gameObject.SetActive(false);
            m_currentMenu = null;
            GainFocus();
        }
    }
	
    public void StartPressed()
    {
        m_household.Paused = false;
        m_cameraFollow.Override = null;
        gameObject.SetActive(false);
        m_household.EmotionalState.OnDepression.AddListener((TaskBroadcaster source) =>
        {
            m_household.Paused = true;
            CloseMenu();

            m_gameOverMenu.GetComponent<GameOverMenu>().Source = source;
            OpenMenu(m_gameOverMenu);            
        });
    }

    public void HowToPlayPressed()
    {
        OpenMenu(m_howToPlayMenu);
    }

    public void QuitPressed()
    {
        Application.Quit();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
