using System.Collections;
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

    void Start()
    {
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        musicButton.onClick.AddListener(Music);
        soundEffectsButton.onClick.AddListener(SoundEffects);

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

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void UpdateMusic(bool isOn)
    {
        if(isOn)
        {
            musicText.text = "On";
        } else
        {
            musicText.text = "Off";
        }
    }

    private void UpdateSoundEffects(bool isOn)
    {
        if (isOn)
        {
            soundEffectsText.text = "On";
        }
        else
        {
            soundEffectsText.text = "Off";
        }
    }
}
