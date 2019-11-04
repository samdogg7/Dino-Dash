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
    //Add button listeners
    void Start()
    {
        startButton.onClick.AddListener(StartButtonClicked);
        settingsButton.onClick.AddListener(SettingsButtonClicked);
        leaderboardButton.onClick.AddListener(LeaderboardButtonClicked);
        Time.timeScale = 1f;
    }
    //Start game scene
    void StartButtonClicked()
    {
        loadingScene.LoadlLevel(2);
    }
    //Goto settings scene
    void SettingsButtonClicked()
    {
        SceneManager.LoadScene("SettingsScene");
    }
    //Goto settings scene
    void LeaderboardButtonClicked()
    {
        LeaderboardManager.instance.DisplayNativeLeaderboard();
    }
}