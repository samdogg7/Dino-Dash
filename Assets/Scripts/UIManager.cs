﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI score;
    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject pauseOverlay;
    public GameObject gameoverCanvas;
    public GameObject mainMenuButtonGameOver;
    public GameObject restartButton;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pauseButton.GetComponent<Button>().onClick.AddListener(PauseClicked);
        playButton.GetComponent<Button>().onClick.AddListener(PlayClicked);
        mainMenuButtonGameOver.GetComponent<Button>().onClick.AddListener(MainMenuClicked);
        restartButton.GetComponent<Button>().onClick.AddListener(PlayClicked);
    }

    void Update()
    {
        score.text = "Score: " + GameManager.instance.score;
    }

    public void UpdateHunger()
    {

    }

    void PauseClicked()
    {
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        pauseOverlay.SetActive(true);
        Time.timeScale = 0f;
    }

    void PlayClicked()
    {
        playButton.SetActive(false);
        pauseButton.SetActive(true);
        pauseOverlay.SetActive(false);
        Time.timeScale = 1f;
    }

    void MainMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    void RestartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        gameoverCanvas.SetActive(true);
    }
}
