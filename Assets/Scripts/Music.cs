using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Music : MonoBehaviour
{
    public AudioSource musicAudioSource;

    private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

    private void Update()
    {
        if(Settings.instance.music && !musicAudioSource.isPlaying)
        {
            musicAudioSource.Play();
        } else if(!Settings.instance.music && musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }
    }
}
