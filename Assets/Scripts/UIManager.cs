using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI score;
    public Button playButton;
    public Button pauseButton;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        score.text = "Score: " + GameManager.instance.score;
    }
}
