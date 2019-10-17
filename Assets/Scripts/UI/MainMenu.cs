using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button settingsButton;
    //Add button listeners
    void Start()
    {
        startButton.onClick.AddListener(StartButtonClicked);
        settingsButton.onClick.AddListener(SettingsButtonClicked);
        Time.timeScale = 1f;
    }
    //Start game scene
    void StartButtonClicked()
    {
        //if(Settings.instance.isTutorial)
        //{
        //    SceneManager.LoadScene("TutorialScene");
        //}
        //else
        //{
        //    SceneManager.LoadScene("GameScene");
        //}

        SceneManager.LoadScene("GameScene");
    }
    //Goto settings scene
    void SettingsButtonClicked()
    {
        SceneManager.LoadScene("SettingsScene");
    }
}