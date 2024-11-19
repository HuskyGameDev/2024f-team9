using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip1;
    public AudioClip clip2;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = clip1;
        audioSource.Play();
    }

     void Update()
    {
        // Check if the first clip is finished playing
        if (!audioSource.isPlaying && audioSource.clip == clip1)
        {
            OnAudioClipEnd();
        }
    }
    void OnAudioClipEnd()
    {
        // Play the second audio clip once the first one ends
        audioSource.clip = clip2;
        audioSource.loop = true;
        audioSource.Play();
    }
}
