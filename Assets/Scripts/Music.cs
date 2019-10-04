using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Music : MonoBehaviour
{
	private static Music audioSource = null;
    public static Music Instance
	{
        get { return audioSource; }
	}

    private void Awake()
	{
		if (audioSource != null && audioSource != this) {
			Destroy(this.gameObject);
			return;
		}
		else
		{
			audioSource = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}
}
