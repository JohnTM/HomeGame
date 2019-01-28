using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class EmotionalStateUI : MonoBehaviour
{
    public class OnDepressionEvent : UnityEvent<TaskBroadcaster> {}


    [SerializeField]
    private OnDepressionEvent m_onDepression = new OnDepressionEvent();

    public OnDepressionEvent OnDepression
    {
        get
        {
            return m_onDepression;
        }
    }

    [SerializeField]
    private Sprite[] m_sprites;

    [SerializeField]
    private Image m_fill;

    [SerializeField]
    private Gradient m_fillGradient;

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

        float t = (m_currentScore / 100.0f);

        image.sprite = m_sprites[(int)(t * (m_sprites.Length-1))];

        m_fill.fillAmount = t;
        m_fill.color = m_fillGradient.Evaluate(t);
        m_fill.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
       
	}

    public void Depressed(TaskBroadcaster source)
    {
        m_onDepression.Invoke(source);
    }

    public void Reward(int amount, TaskBroadcaster source)
    {
        if (m_currentScore == 0)
        {
            return;
        }

        m_currentScore = Mathf.Min(m_currentScore + amount, 100);        

        var rt = GetComponent<RectTransform>();

        if (m_currentTweener != null)
        {
            m_currentTweener.Kill(true);
        }

        DOTween.Kill(rt);
        m_currentTweener = rt.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f),
            0.7f).OnComplete(() => { });
    }

    public void Punish(int amount, TaskBroadcaster source)
    {        
        if (m_currentScore == 0)
        {
            return;
        }

        if (m_currentScore > 0 && m_currentScore - amount <= 0)
        {
            Depressed(source);
            return;
        }

        m_currentScore = Mathf.Max(m_currentScore - amount, 0);

        var rt = GetComponent<RectTransform>();
        
        if (m_currentTweener != null)
        {
            m_currentTweener.Kill(true);
        }

        Vector3 v = new Vector3(Random.Range(-10.0f, -3.0f), Random.Range(-5.0f, -2.0f), 0.0f);
        m_currentTweener = rt.DOPunchPosition(v,
            0.7f).OnComplete(() => { transform.position = m_startPos; });
    }
}
