using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using UnityEngine.SocialPlatforms;

public class LeaderboardManager : MonoBehaviour
{
    private static LeaderboardManager _instance;
    public static LeaderboardManager instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    void Start()
    {
        GameServices.ManagedInit();
    }

    public void DisplayNativeLeaderboard()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.ShowLeaderboardUI(EM_GameServicesConstants.Leaderboard_DDLeaderboard);
        }
    }

    public void AddScore(int score)
    {
        if(GameServices.IsInitialized())
        {
            GameServices.ReportScore(score, EM_GameServicesConstants.Leaderboard_DDLeaderboard);
        }
    }

    public void GetLeaderboardValues(int startingFrom, int count)
    {
        GameServices.LoadScores(EM_GameServicesConstants.Leaderboard_DDLeaderboard, startingFrom, count, TimeScope.AllTime, UserScope.Global, OnScoresLoaded);
    }

    private void OnScoresLoaded(string leaderboardName, IScore[] scores)
    {
        if (scores != null && scores.Length > 0)
        {
            foreach (IScore score in scores)
            {
                //Handle scores
            }
        }
    }
}
