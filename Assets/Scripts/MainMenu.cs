using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button settingsButton;

    void Start()
    {
        startButton.onClick.AddListener(StartButtonClicked);
        settingsButton.onClick.AddListener(SettingsButtonClicked);
    }

    void StartButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

    void SettingsButtonClicked()
    {
        SceneManager.LoadScene("SettingsScene");
    }
}