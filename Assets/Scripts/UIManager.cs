using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public SpriteRenderer healthSpriteRender;
    public TextMeshProUGUI inGameScore;
    public TextMeshProUGUI scoreGameOver;
    public TextMeshProUGUI highscoreGameOver;
    public TextMeshProUGUI hunger;
    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject pauseOverlay;
    public GameObject gameoverCanvas;
    public GameObject mainMenuButtonGameOver;
    public GameObject restartButton;

    //instance variable
    private void Awake()
    {
        instance = this;
    }

    //Setup game buttons for gameover and pause
    void Start()
    {
        pauseButton.GetComponent<Button>().onClick.AddListener(PauseClicked);
        playButton.GetComponent<Button>().onClick.AddListener(PlayClicked);
        mainMenuButtonGameOver.GetComponent<Button>().onClick.AddListener(MainMenuClicked);
        restartButton.GetComponent<Button>().onClick.AddListener(RestartClicked);
    }

    //Update score every frame
    void Update()
    {
        inGameScore.text = "Score: " + GameManager.instance.score;
        scoreGameOver.text = "Score: " + GameManager.instance.score;
    }

    //Update the hunger int on screen
    public void UpdateHunger(int dinoHunger)
    {
        hunger.text = dinoHunger.ToString();
        if(dinoHunger >= 0)
        {
            //string spriteName = "hp bar " + Mathf.Abs(dinoHunger - 50).ToString() + " of 50";
            int difference = 50 - dinoHunger;

            Debug.Log(difference.ToString());
            string spriteName = "hp bar " + difference.ToString() + " of 50";
            healthSpriteRender.sprite = Resources.Load<Sprite>("50 HP bar/" + spriteName);
        }
    }

    //Handle pause being clicked, the pause overlay causes the screen to get alittle darker to indicate to the user that they are paused
    //Time.timeScale can be used for slow motion and freezing a scene. Rather than checking every moving object with a bool, we use this instead
    void PauseClicked()
    {
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        pauseOverlay.SetActive(true);
        Time.timeScale = 0f;
    }

    //Handle play being clicked, turn off the pause overlay causes the screen to get alittle darker to indicate to the user that they are paused
    //Time.timeScale can be used for slow motion and freezing a scene. Rather than checking every moving object with a bool, we use this instead
    void PlayClicked()
    {
        playButton.SetActive(false);
        pauseButton.SetActive(true);
        pauseOverlay.SetActive(false);
        Time.timeScale = 1f;
    }

    //Go back to the main menu
    void MainMenuClicked()
    {
        SceneManager.LoadScene(0);
    }

    //Restart the game
    void RestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Update the gameover text
    public void GameOver(int highscore)
    {
        if(highscore > 0)
        {
            highscoreGameOver.text = "Your highscore: " + highscore;
        } else
        {
            highscoreGameOver.text = "";
        }
        gameoverCanvas.SetActive(true);
    }
}
