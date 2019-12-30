using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;

public class GameManager : MonoBehaviour
{
    public GameObject selectedCharacter;
    public bool isAlive = true;
    //Tile movement speed
    public float tileMovementSpeed = 2f;
    //Score is located in game manager because it is easily accesible for all game objects
    public int score;
    private bool submittedScore = false;

    private static GameManager _instance;
    public static GameManager instance { get { return _instance; } }

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

    //Make sure time scale is at 1f
    private void Start()
    {
        InvokeRepeating("AddOneScore", 0f, 1f);
        if (Settings.instance != null) {
            if (!Settings.instance.isTutorial) {
                Time.timeScale = 1f;
            }
        } else
        {
            Time.timeScale = 1f;
        }
    }

    private void AddOneScore()
    {
        if (!Settings.instance.isTutorial) {
            score += 1;
        }
    }

    //Handle player !isAlive, update player prefs with high score and update UI
    private void Update()
    {
        if (!isAlive)
        {
            Time.timeScale = 0f;
            //Save locally the users highscore
            if(PlayerPrefs.HasKey("highscore"))
            {
                int oldHighScore = PlayerPrefs.GetInt("highscore");
                if(oldHighScore < score)
                {
                    PlayerPrefs.SetInt("highscore", score);
                }
                UIManager.instance.GameOver(oldHighScore);
            } else
            {
                UIManager.instance.GameOver(0);
                PlayerPrefs.SetInt("highscore", score);
            }
            if(LeaderboardManager.instance != null && !submittedScore)
            {
                LeaderboardManager.instance.AddScore(score);
                submittedScore = true;
            }
        }
    }
}
