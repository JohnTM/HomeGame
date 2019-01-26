using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCollisionSounds : MonoBehaviour {

    public bool differentAudioFirstUse = true;
    [Tooltip("This is only needed if you are having first use sounds")]
    public List<AudioClip> firstUseAudioClips = new List<AudioClip>();
    bool alreadyPlayed = false;

    public List <AudioClip> audioclips = new List<AudioClip>();
    public float volMultipler = 1f;
    float volume;

    private void OnCollisionEnter(Collision collision)
    {
        volume = collision.relativeVelocity.magnitude;
        //Messy, just playing sound at location of main camera, didn't want to add audiosources to every object that could make sound but wanted them to always be audible
        if (differentAudioFirstUse && !alreadyPlayed)
        {
            Debug.Log("Should be playing sound");
            AudioSource.PlayClipAtPoint(firstUseAudioClips[Random.Range(0, firstUseAudioClips.Count)], Camera.main.transform.position, 0.3f);
            alreadyPlayed = true;
        }
        else
        {
            AudioSource.PlayClipAtPoint(audioclips[Random.Range(0, audioclips.Count)], Camera.main.transform.position, volume * volMultipler);
        }
    }
}
