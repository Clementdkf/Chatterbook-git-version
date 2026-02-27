using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextToSpeech : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip textToSpeechClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayTextClip()
    {
        if (audioSource != null && textToSpeechClip != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(textToSpeechClip);
        }
    }

}
