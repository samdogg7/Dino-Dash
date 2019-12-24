using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager instance { get { return _instance; } }

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
        }
    }


    public AudioSource munchSound;
    public AudioSource crispSound;
    public AudioSource meteorSound;

    public void PlayMunchSound()
    {
        if(!munchSound.isPlaying)
        {
            munchSound.Play();
        }
    }

    public void PlayCrispSound()
    {
        if (!crispSound.isPlaying)
        {
            crispSound.Play();
        }
    }

    public void PlayMeteorSound()
    {
        if (!meteorSound.isPlaying)
        {
            meteorSound.Play();
        }
    }
}
