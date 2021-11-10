using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }
    public List<AudioClip> audioClips = new List<AudioClip>();
    public AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = this.GetComponent<AudioSource>();
    }

    public void PlayButtonSound()
    {

        audioSource.PlayOneShot(audioClips[0]);
    }

    public void PlayErrorSound()
    {
        audioSource.PlayOneShot(audioClips[1]);
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(audioClips[2]);
    }
}
