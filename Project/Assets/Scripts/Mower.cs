using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Mower : MonoBehaviour {

    [SerializeField]
    private Transform m_body;

    [SerializeField]
    private AudioClip[] m_clips;

    private AudioSource m_audioSource1;
    private AudioSource m_audioSource2;

    private Sequence m_turnOnSeq;
    private Sequence m_turnOffSeq;

    // Use this for initialization
    void Start () {
        m_audioSource1 = gameObject.AddComponent<AudioSource>();
        m_audioSource2 = gameObject.AddComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {		
	}

    public void On()
    {
        if (m_turnOffSeq != null)
        {
            m_turnOffSeq.Kill();
            m_turnOffSeq = null;
        }

        m_body.DOShakePosition(1.0f, 0.1f, 20, 90, false, false).SetLoops(-1);

        m_turnOnSeq = DOTween.Sequence();        
        m_turnOnSeq.AppendCallback(() => 
        {
            m_audioSource1.volume = 1.0f;
            m_audioSource1.clip = m_clips[0];
            m_audioSource1.Play(); });
        m_turnOnSeq.AppendInterval(m_clips[0].length - 1.0f);
        m_turnOnSeq.AppendCallback(() => 
        {
            m_audioSource2.clip = m_clips[1];
            m_audioSource2.volume = 0.0f;
            m_audioSource2.Play();
            m_audioSource2.loop = true;
        }).Append(m_audioSource2.DOFade(1.0f, 1.0f));
    }

    public void Off()
    {
        m_body.DOKill();

        if (m_turnOnSeq != null)
        {
            m_turnOnSeq.Kill();
            m_turnOnSeq = null;

            m_turnOffSeq = DOTween.Sequence();
            m_turnOnSeq.Append(m_audioSource2.DOFade(0.0f, 1.0f));
        }
    }

}
