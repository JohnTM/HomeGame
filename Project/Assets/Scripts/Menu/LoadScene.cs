using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    [SerializeField]
    private int m_sceneIndex;

    public void GoToScene()
    {
        SceneManager.LoadScene(m_sceneIndex);
    }
}
