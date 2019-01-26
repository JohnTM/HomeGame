using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EmotionalStateUI : MonoBehaviour
{

    [SerializeField]
    private Sprite[] m_sprites;

    [SerializeField, Range(0, 100)]
    private int m_currentScore = 100;

    private Tweener m_currentTweener;

    private Vector3 m_startPos;

	// Use this for initialization
	void Start () {
        m_startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Image image = GetComponent<Image>();
        image.sprite = m_sprites[(int)((m_currentScore / 100.0f) * (m_sprites.Length-1))];
	}

    public void Punish(int amount, TaskBroadcaster source)
    {
        m_currentScore = Mathf.Max(m_currentScore - amount, 0);

        //if (m_currentTweener != null)
        //{
        //    m_currentTweener.Complete();
        //}
        
        var rt = GetComponent<RectTransform>();
        Vector2 v = new Vector2(Random.Range(-10.0f, -3.0f), Random.Range(-5.0f, -2.0f));
        m_currentTweener = rt.DOPunchAnchorPos(v,
            0.7f).ChangeStartValue(m_startPos);
    }
}
