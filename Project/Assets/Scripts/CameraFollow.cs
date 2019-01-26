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


    private Vector3 m_initialPosition;
    private Vector3 m_cameraVelocity;

    private Player[] m_players;

	// Use this for initialization
	void Start ()
    {
        m_players = FindObjectsOfType<Player>();
        m_initialPosition = transform.position;

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


        transform.position = Vector3.SmoothDamp(transform.position, target, ref m_cameraVelocity, 0.5f);


	}
}
