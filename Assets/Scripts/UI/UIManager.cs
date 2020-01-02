using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


//This Script Manages all the UI in the game keeping track of scores and different
//variable states.
public class UIManager : MonoBehaviour
{
	public SpriteRenderer healthSpriteRender;
    public TextMeshProUGUI inGameScore;
    public TextMeshProUGUI scoreGameOver;
    public TextMeshProUGUI highscoreGameOver;
    public GameObject pauseScreen;
    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject gameoverCanvas;
    public GameObject mainMenuButtonPause;
    public GameObject restartButtonPause;
    public GameObject mainMenuButtonGameOver;
    public GameObject restartButtonGameOver;
    public bool paused = false;

	private static UIManager _instance;
	public static UIManager instance { get { return _instance; } }

	//instance variable
	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}
	}

	//Setup game buttons for gameover and pause
	void Start()
    {
        pauseButton.GetComponent<Button>().onClick.AddListener(PauseClicked);
        playButton.GetComponent<Button>().onClick.AddListener(PlayClicked);
        mainMenuButtonPause.GetComponent<Button>().onClick.AddListener(MainMenuClicked);
        restartButtonPause.GetComponent<Button>().onClick.AddListener(RestartClicked);
        mainMenuButtonGameOver.GetComponent<Button>().onClick.AddListener(MainMenuClicked);
        restartButtonGameOver.GetComponent<Button>().onClick.AddListener(RestartClicked);
    }

    //Update score every frame
    void Update()
    {
        inGameScore.text = "Score " + GameManager.instance.score;
        scoreGameOver.text = "Score: " + GameManager.instance.score;
    }

    //Update the hunger on screen
    public void UpdateHunger(int dinoHunger, int totalHunger)
    {
        if(dinoHunger >= 0)
        {
            //difference will take whatever the total hunger is and find the multiple that will make the hunger fit within 50 (there are 50 health bar sprites)
            int difference = totalHunger / 50;
            int healthBarSpriteIndex = (totalHunger / difference) - (dinoHunger/ difference);
            string spriteName = "hp bar " + healthBarSpriteIndex.ToString() + " of 50";
            healthSpriteRender.sprite = Resources.Load<Sprite>("50 HP bar/" + spriteName);
        }
    }

    //Handle pause being clicked, the pause overlay causes the screen to get alittle darker to indicate to the user that they are paused
    //Time.timeScale can be used for slow motion and freezing a scene. Rather than checking every moving object with a bool, we use this instead
    void PauseClicked()
    {
        paused = true;
        pauseButton.SetActive(false);
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    //Handle play being clicked, turn off the pause overlay causes the screen to get alittle darker to indicate to the user that they are paused
    //Time.timeScale can be used for slow motion and freezing a scene. Rather than checking every moving object with a bool, we use this instead
    void PlayClicked()
    {
        paused = false;
		pauseScreen.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    //Go back to the main menu
    void MainMenuClicked()
    {
        SceneManager.LoadScene(1);
    }

    //Restart the game
    void RestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Update the gameover text
    public IEnumerator GameOver(int highscore)
    {
        yield return new WaitForSeconds(1f);

        if (highscore > 0)
        {
            highscoreGameOver.text = "Personal highscore: " + highscore;
        }
        else
        {
            highscoreGameOver.text = "";
        }
        gameoverCanvas.SetActive(true);
    }
}
