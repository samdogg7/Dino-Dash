using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScript : MonoBehaviour
{
    public Button menuButton;
    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        menuButton.onClick.AddListener(MenuButtonClicked);
        restartButton.onClick.AddListener(RestartButtonClicked);
    }

    void MenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    void RestartButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMenu()
    {
        
    }
}
