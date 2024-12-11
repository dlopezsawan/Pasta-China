using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudio : MonoBehaviour
{
    private AudioSource audioSource;
    AudioManager audioManager;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void PlaySound(string audioName, string clipName)
    {
        // Play the fire sound
        if (audioSource==null) { return; }
        audioManager.PlayAudio(audioSource, audioName, clipName, false);
    }

    public void StopSound()
    {
        // Stop the current sound
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
