﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class SettingsHandler : MonoBehaviour
{
    public Button musicButton;
    public TextMeshProUGUI musicText;
    public Button soundEffectsButton;
    public TextMeshProUGUI soundEffectsText;
    public Button mainMenuButton;
    public Button tutorialButton;


    void Start()
    {
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        musicButton.onClick.AddListener(Music);
        soundEffectsButton.onClick.AddListener(SoundEffects);
        tutorialButton.onClick.AddListener(Tutorial);

        if (Settings.instance != null)
        {
            UpdateMusic(Settings.instance.music);
            UpdateSoundEffects(Settings.instance.soundEffects);
        }
    }

    private void Music()
    {
        if(Settings.instance != null)
        {
            Settings.instance.music = !Settings.instance.music;
            UpdateMusic(Settings.instance.music);
        }
    }

    private void SoundEffects()
    {
        if (Settings.instance != null)
        {
            Settings.instance.soundEffects = !Settings.instance.soundEffects;
            UpdateSoundEffects(Settings.instance.soundEffects);
        }
    }

    private void Tutorial()
    {
        Settings.instance.isTutorial = true;
        PlayerPrefs.DeleteKey("Tutorial");
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void UpdateMusic(bool isOn)
    {
        if(isOn)
        {
            musicText.text = "On";
            PlayerPrefs.SetInt("Music", 1);
        } else
        {
            PlayerPrefs.SetInt("Music", 0);
            musicText.text = "Off";
        }
    }

    private void UpdateSoundEffects(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("SoundEffects", 1);
            soundEffectsText.text = "On";
        }
        else
        {
            PlayerPrefs.SetInt("SoundEffects", 1);
            soundEffectsText.text = "Off";
        }
    }
}
