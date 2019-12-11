using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Music : MonoBehaviour
{
    public AudioSource musicAudioSource;

	private static Music _instance;
	public static Music instance { get { return _instance; } }

	//instance variable
	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
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
