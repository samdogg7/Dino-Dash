using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI score;
    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject pauseOverlay;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pauseButton.GetComponent<Button>().onClick.AddListener(PauseClicked);
        playButton.GetComponent<Button>().onClick.AddListener(PlayClicked);
    }

    void Update()
    {
        score.text = "Score: " + GameManager.instance.score;
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

    public void GameOver()
    {

    }
}
