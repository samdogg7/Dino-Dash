using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject selectedCharacter;

    public bool isAlive = true;

    public float tileMovementSpeed = 2f;
    public int score;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating("AddOneScore", 0f, 1f);
        Time.timeScale = 1f;
    }

    private void AddOneScore()
    {
        score += 1;
    }

    private void Update()
    {
        if (!isAlive)
        {
            UIManager.instance.GameOver();
            Time.timeScale = 0f;
        }
    }
}
