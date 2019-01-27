using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipHolder : MonoBehaviour {

    public AudioClip clip1;

    public AudioClip clip2;

    public AudioClip clip3;

    public AudioClip clip4;

    public AudioClip clip5;



    public void PlayAudio(int clipNum)
    {
        AudioClip clip;
        switch (clipNum)
        {
            case 1:
                clip = clip1;
                break;
            case 2:
                clip = clip2;
                break;
            case 3:
                clip = clip3;
                break;
            case 4:
                clip = clip4;
                break;
            case 5:
                clip = clip5;
                break;
            default:
                Debug.Log("Invalid clip num");
                return;
        }
        //audioSource.PlayOneShot(clip);
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }
}

