﻿using System.Collections;
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
    private Image m_contextImage;

    private Rigidbody m_rigidbody;

    private ContextAction m_closestAction;

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

	// Use this for initialization
	void Start ()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_contextImage.transform.parent.parent = null;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        TwoAxisInputControl movement = MovementControl;

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

        if (minAction)
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