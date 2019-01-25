using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int m_playerIndex;

    [SerializeField]
    private float m_moveSpeed = 1;

    [SerializeField]
    private Transform m_pickupPoint;

    [SerializeField]
    private Image m_contextImage;

    private Rigidbody m_rigidbody;

    private ContextAction m_closestAction;

    public InputControl ActionControl
    {
        get
        {
            if (InputManager.Devices.Count == 1)
            {
                if (m_playerIndex == 0)
                {
                    return InputManager.Devices[0].LeftTrigger;
                }
                else if (m_playerIndex == 1)
                {
                    return InputManager.Devices[0].RightTrigger;
                }
            }
            else if (InputManager.Devices.Count > m_playerIndex)
            {
                return InputManager.Devices[m_playerIndex].Action1;
            }
            return null;
        }
    }


    public TwoAxisInputControl MovementControl
    {
        get
        {
            if (InputManager.Devices.Count == 1)
            {
                if (m_playerIndex == 0)
                {
                    return InputManager.Devices[0].LeftStick;
                }
                else if (m_playerIndex == 1)
                {
                    return InputManager.Devices[0].RightStick;
                }                
            }
            else if (InputManager.Devices.Count > m_playerIndex)
            {
                return InputManager.Devices[m_playerIndex].LeftStick;
            }
            return null;
        }
    }
   

    public InputDevice Device
    {
        get
        {
            if (InputManager.Devices.Count > m_playerIndex)
            {
                return InputManager.Devices[m_playerIndex];
            }
            
            return null;            
        }
    }

    public Item CurrentItem
    {
        get
        {
            return m_currentItem;
        }

        set
        {
            if (m_currentItem == null && value != null)
            {
                m_currentItem = value;
                m_currentItem.transform.parent = m_pickupPoint;
                m_currentItem.transform.localPosition = Vector3.zero;
                m_currentItem.Pickup(this);
            }

            if (m_currentItem && value == null)
            {
                m_currentItem.transform.parent = null;
                m_currentItem.Drop(this);
                m_currentItem = null;
            }
        }
    }

    private Item m_currentItem;

	// Use this for initialization
	void Start ()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_contextImage.transform.parent.parent = null;
	}

    // Update is called once per frame
    void Update()
    {
        TwoAxisInputControl movement = MovementControl;
        InputControl actionButton = ActionControl;

        if (actionButton != null && m_closestAction)
        {
            bool shouldTrigger = false;

            if (m_closestAction.Type == ContextAction.TriggerType.Instant)
            {
                if (actionButton.WasPressed)
                {
                    shouldTrigger = true;
                }
            }
            else
            {
                if (actionButton.IsPressed)
                {
                    shouldTrigger = true;
                }
            }

            if (shouldTrigger)
            {
                ContextAction.TriggerPhase phase = m_closestAction.Trigger(this);
                switch (phase)
                {
                    case ContextAction.TriggerPhase.Ended:
                        break;
                    case ContextAction.TriggerPhase.InProgress:
                        // TODO: progress indicator
                        break;
                }
            }
        }

        if (movement != null)
        {
            // TODO: Handle movement
            if (movement.Vector != Vector2.zero)
            {
                float angle = movement.Angle;
                m_rigidbody.MoveRotation(Quaternion.Euler(0, 180 + angle, 0));               

                Vector2 dir = movement.Vector;
                m_rigidbody.MovePosition(m_rigidbody.position + new Vector3(dir.x, 0, dir.y) * m_moveSpeed * Time.deltaTime);
            }
        }

        CheckForContextActions();
    }

    private void CheckForContextActions()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, 1);
        float minDist = float.PositiveInfinity;
        ContextAction minAction = null;

        foreach (Collider c in objects)
        {
            float dist = Vector3.Distance(c.gameObject.transform.position, transform.position);
            if (minAction == null || dist < minDist)
            {
                ContextAction action = c.GetComponentInParent<ContextAction>();
                if (action != null && action.isActiveAndEnabled)
                {
                    minAction = action;
                }
            }
        }

        if (minAction && minAction.CanTrigger(this))
        {
            m_closestAction = minAction;
            m_contextImage.enabled = true;

            Transform canvasT = m_contextImage.transform.parent;
            canvasT.position = minAction.transform.position + Vector3.up * 1.0f;

            Vector3 camLook = (Camera.main.transform.position - canvasT.position).normalized;

            m_contextImage.transform.parent.rotation = Quaternion.LookRotation(camLook);
        }
        else
        {
            m_closestAction = null;
            m_contextImage.enabled = false;
        }

    }
}
