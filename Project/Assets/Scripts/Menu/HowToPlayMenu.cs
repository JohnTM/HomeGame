using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HowToPlayMenu : MonoBehaviour {

    [SerializeField]
    private Transform m_goBackButton;
    
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(m_goBackButton.gameObject);
    }
}
