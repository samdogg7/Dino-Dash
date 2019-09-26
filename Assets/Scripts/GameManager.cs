using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject selectedCharacter;
    public bool isAlive = true;

    private void Awake()
    {
        Instance = this;

    }
}
