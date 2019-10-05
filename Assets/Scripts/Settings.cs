﻿using System.Collections;
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
    public static Settings instance;

    //Each array of sprites for varying colors
    public Sprite[] greenRunningSprites;
    public Sprite[] redRunningSprites;
    public Sprite[] yellowRunningSprites;
    public Sprite[] blueRunningSprites;

    public DinoColor dinoColor = DinoColor.Green;

    public bool music = true;
    public bool soundEffects = true;

    //Make sure settings exists in all scenes
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
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
