using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Vacuum : MonoBehaviour {
    [SerializeField]
    private Transform m_body;

    [SerializeField]
    private AudioClip[] m_clips;

    private AudioSource m_audioSource1;

    private Sequence m_turnOnSeq;

    private Vector3 m_bodyOrigin;

    // Use this for initialization
    void Start()
    {
        m_audioSource1 = gameObject.AddComponent<AudioSource>();
        m_bodyOrigin = m_body.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void On()
    {
        if (m_turnOnSeq != null)
        {
            m_turnOnSeq.Kill();
            m_turnOnSeq = null;
        }

        m_body.DOShakePosition(1.0f, 0.05f, 20, 90, false, false).SetLoops(-1);

        m_turnOnSeq = DOTween.Sequence();
        m_turnOnSeq.AppendCallback(() =>
        {
            m_audioSource1.volume = 0.0f;
            m_audioSource1.clip = m_clips[0];
            m_audioSource1.Play();
        });
        m_turnOnSeq.Append(m_audioSource1.DOFade(1.0f, 1.0f));
    }

    public void Off()
    {
        m_body.DOKill();

        if (m_turnOnSeq != null)
        {
            m_turnOnSeq.Kill();
            m_turnOnSeq = null;
        }

        m_turnOnSeq = DOTween.Sequence();
        m_turnOnSeq.Append(m_audioSource1.DOFade(0.0f, 1.0f));

        m_body.localPosition = m_bodyOrigin;
    }
}
