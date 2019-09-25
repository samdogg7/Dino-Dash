using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button settingsButton;

    void Update()
    {
        if(startButton.enabled)
        {
            SceneManager.LoadScene("GameScene");
        }
        if (settingsButton.enabled)
        {
            SceneManager.LoadScene("SettingsScene");
        }
    }
}
