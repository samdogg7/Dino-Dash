using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public LoadingScene loadingScene;
    public Button startButton;
    public Button settingsButton;
    public Button leaderboardButton;

    private bool isLoading = false;

    //Add button listeners
    void Start()
    {
        startButton.onClick.AddListener(StartButtonClicked);
        settingsButton.onClick.AddListener(SettingsButtonClicked);
        leaderboardButton.onClick.AddListener(LeaderboardButtonClicked);
        Time.timeScale = 1f;
        isLoading = false;
    }

    //Start game scene
    void StartButtonClicked()
    {
        if(!isLoading)
        {
            loadingScene.LoadlLevel(2);
            isLoading = true;
        }
    }
    //Goto settings scene
    void SettingsButtonClicked()
    {
        if (!isLoading)
        {
            SceneManager.LoadScene("SettingsScene");
            isLoading = true;
        }
    }
    //Goto settings scene
    void LeaderboardButtonClicked()
    {
        LeaderboardManager.instance.DisplayNativeLeaderboard();
    }
}