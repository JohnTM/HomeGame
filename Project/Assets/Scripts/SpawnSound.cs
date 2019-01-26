using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSound : MonoBehaviour {

    public List<AudioClip> audioclips = new List<AudioClip>();
    [Range(0f, 1f)]
    public float vol = 1f;

    // Use this for initialization
    void Start () {
        AudioSource.PlayClipAtPoint(audioclips[Random.Range(0, audioclips.Count)], Camera.main.transform.position, vol);
    }
	
}
