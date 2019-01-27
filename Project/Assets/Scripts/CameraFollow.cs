using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float m_minCameraDistance = 10;
    [SerializeField]
    private float m_minPlayerDistance = 5;
    [SerializeField]
    private float m_maxPlayerDistance = 10;

    [SerializeField]
    private Vector3 m_minCameraBounds;

    [SerializeField]
    private Vector3 m_maxCameraBounds;

    public Transform Override;

    private Vector3 m_initialPosition;
    private Quaternion m_initialRotation;
    private Vector3 m_cameraVelocity;
    private float m_cameraAngularVelocity;

    private Player[] m_players;

	// Use this for initialization
	void Start ()
    {
        m_players = FindObjectsOfType<Player>();
        m_initialPosition = transform.position;
        m_initialRotation = transform.rotation;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 averagePosition = Vector3.zero;
        foreach (var p in m_players)
        {
            averagePosition += p.transform.position;
        }
        averagePosition /= m_players.Length;

        float playerDistance = Vector3.Distance(m_players[0].transform.position, m_players[1].transform.position);

        float followFactor = Mathf.Clamp01((playerDistance - m_minPlayerDistance) / (m_maxPlayerDistance - m_minPlayerDistance));

        Vector3 target = Vector3.Lerp(averagePosition - transform.forward * m_minCameraDistance, m_initialPosition, followFactor);
        target.x = Mathf.Clamp(target.x, m_minCameraBounds.x, m_maxCameraBounds.x);
        target.y = Mathf.Clamp(target.y, m_minCameraBounds.y, m_maxCameraBounds.y);
        target.z = Mathf.Clamp(target.z, m_minCameraBounds.z, m_maxCameraBounds.z);

        Quaternion targetRotation = m_initialRotation;

        if (Override)
        {
            target = Override.position;
            targetRotation = Override.rotation;
        }

        transform.position = Vector3.SmoothDamp(transform.position, target, ref m_cameraVelocity, 0.5f, 8*2, Time.unscaledDeltaTime);

        // https://answers.unity.com/questions/390291/is-there-a-way-to-smoothdamp-a-lookat.html
        var delta = Quaternion.Angle(transform.rotation, targetRotation);
        if (delta > 0.0f)
        {
            var t = Mathf.SmoothDampAngle(delta, 0.0f, ref m_cameraAngularVelocity, 0.5f, 18*2, Time.unscaledDeltaTime);
            t = 1.0f - t / delta;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
        }
        
	}
}
