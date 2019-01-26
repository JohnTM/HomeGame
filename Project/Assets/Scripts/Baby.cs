using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Baby : MonoBehaviour {

    [SerializeField]
    private float m_wanderRadius = 1.0f;
    [SerializeField]
    private float m_wanderTime = 1.0f;
    [SerializeField]
    private float m_wanderTimeVariance = 0.25f;

    [SerializeField]
    private float m_cryChanceBase = 0.05f;

    [SerializeField]
    private float m_cryChanceAloneModifier = 0.15f;

    [SerializeField]
    private AudioSource m_cryAudioSource;

    [SerializeField]
    private AudioSource m_noisesAudioSource;


    private float m_timer;

    private NavMeshAgent m_agent;

    private TaskBroadcaster m_tb;

    public bool IsCrying
    {
        get
        {
            return m_tb.Active;
        }
    }


    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_tb = GetComponent<TaskBroadcaster>();
    }

    // Use this for initialization
    void Start () {
        m_noisesAudioSource.time = Random.Range(0, m_noisesAudioSource.clip.length);
        m_noisesAudioSource.Play();        
    }

    public void Cry()
    {
        if (!IsCrying)
        {
            m_tb.Activate();            
            m_noisesAudioSource.mute = true;
            m_cryAudioSource.time = Random.Range(0, m_cryAudioSource.clip.length);
            m_cryAudioSource.Play();
            m_cryAudioSource.volume = 0;
            m_cryAudioSource.DOFade(1.0f, 0.7f);
        }        
    }

    public void StopCrying()
    {
        if (IsCrying)
        {
            m_tb.Completed();
            m_cryAudioSource.DOFade(0, 0.7f);
            //m_cryAudioSource.Stop();
            //m_noisesAudioSource.time = Random.Range(0, m_noisesAudioSource.clip.length);
            //m_noisesAudioSource.Play();
            
        }        
    }
	
	// Update is called once per frame
	void Update () {

        if (m_agent.enabled)
        {
            m_timer -= Time.deltaTime;

            if (m_timer <= 0)
            {
                m_timer = m_wanderTime + Random.Range(-m_wanderTimeVariance, m_wanderTimeVariance);
                Vector2 dir = Random.insideUnitCircle;
                m_agent.destination = transform.position + new Vector3(dir.x, 0, dir.y) * m_wanderRadius;

                if (Random.value < m_cryChanceBase)
                {
                    Cry();
                }
            }
        }
        else if (IsCrying)
        {
            StopCrying();
        }
    }
}
