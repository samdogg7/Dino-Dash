using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DinoColor
{
    Green,
    Red,
    Yellow,
    Blue
}

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public Button musicOff;
    public Button musicOn;
    public Button soundEffectsOn;
    public Button soundEffectsOff;

    public Sprite[] greenRunningSprites;
    public Sprite[] redRunningSprites;
    public Sprite[] yellowRunningSprites;
    public Sprite[] blueRunningSprites;


    public DinoColor dinoColor = DinoColor.Green;

    private bool music = true;
    private bool soundEffects = true;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        //Make sure not on the main menu
        if(musicOff != null)
        {
            musicOff.onClick.AddListener(MusicOffButtonClicked);
        }
    }

    void MusicOffButtonClicked()
    {
       
    }

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
