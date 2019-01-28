using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using InControl;

public class KitchenDirtSplat : MonoBehaviour
{

    [SerializeField]
    private Transform m_model;

    [SerializeField]
    private float m_minScale = 0.0f;

    [SerializeField]
    private float m_maxScale = 6.0f;

    [SerializeField]
    private float m_growthTime = 60;

    private float m_growthMultiplier;

    private float m_timer = 0;

    private TaskBroadcaster m_tb;

    // Use this for initialization
    void Start()
    {
        m_tb = GetComponent<TaskBroadcaster>();
        m_growthMultiplier = Random.Range(0.8f, 1.2f);
        m_model.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Dirty(float multiplier)
    {
        m_timer += Time.fixedDeltaTime * m_growthMultiplier * multiplier;
        if (m_timer > m_growthTime && m_tb.Active == false)
        {
            m_timer = m_growthTime;
            m_tb.Activate();
        }

        float growth = m_timer / m_growthTime;
        float scale = Mathf.Lerp(m_minScale, m_maxScale, growth);
        m_model.localScale = new Vector3(scale, scale, scale);
    }

    public void Clean()
    {
        m_timer = 0;

        if (m_timer == 0 && m_tb.Active)
        {
            m_tb.Completed();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        // TODO: Vacuum
        if (m_tb)
        {
            if (other.gameObject.GetComponent<Vacuum>())
            {
                Clean();
            }
            else if (other.attachedRigidbody.GetComponent<Player>())
            {
                Player player = other.attachedRigidbody.GetComponent<Player>();

                TwoAxisInputControl movement = player.MovementControl;

                if (movement != null)
                {
                    Dirty(movement.Vector.magnitude * 5.0f);
                }
            }
            else if (other.gameObject.GetComponent<Baby>())
            {
                Dirty(other.GetComponent<NavMeshAgent>().velocity.magnitude + 1);
            }

        }
    }
}
