using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Enum used to select which dino a player selected
public enum DinoColor
{
    Green,
    Red,
    Yellow,
    Blue
}

public class Settings : MonoBehaviour
{
    private static Settings _instance;
    public static Settings instance { get { return _instance; } }

    //Each array of sprites for varying colors
    public Sprite[] greenRunningSprites;
    public Sprite[] redRunningSprites;
    public Sprite[] yellowRunningSprites;
    public Sprite[] blueRunningSprites;

    public DinoColor dinoColor = DinoColor.Green;

    public bool music = true;
    public bool soundEffects = true;
    public bool isTutorial = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    private void Start()
    {
        if(PlayerPrefs.HasKey("Music"))
        {
            int musicVal = PlayerPrefs.GetInt("Music");
            if (musicVal == 0)
            {
                music = false;
            } else
            {
                music = true;
            }
        }

        if(PlayerPrefs.HasKey("SoundEffects"))
        {
            int sfxVal = PlayerPrefs.GetInt("SoundEffects");
            if(sfxVal == 0)
            {
                soundEffects = false;
            } else
            {
                soundEffects = true;
            }
        }

        if (!PlayerPrefs.HasKey("isTutorial"))
        {
            isTutorial = true;
            //fill with anything to make a key
            PlayerPrefs.SetString("isTutorial", "true");
        }
    }

    //Return the selected set of sprites, used to update game character
    public Sprite[] GetRunningSprites()
    {
        if(dinoColor == DinoColor.Blue)
        {
            return blueRunningSprites;
        }
        else if (dinoColor == DinoColor.Red)
        {
            return redRunningSprites;
        }
        else if (dinoColor == DinoColor.Yellow)
        {
            return yellowRunningSprites;
        }
        else
        {
            return greenRunningSprites;
        }
    }
}
