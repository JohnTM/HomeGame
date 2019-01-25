using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int m_playerIndex;

    [SerializeField]
    private float m_moveSpeed = 1;

    private Rigidbody m_rigidbody;

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
    }
}
