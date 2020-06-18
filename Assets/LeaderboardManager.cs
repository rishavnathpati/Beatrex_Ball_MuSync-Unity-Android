using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class LeaderboardManager : MonoBehaviour
{

    public static LeaderboardManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.Activate();
        LogIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
        });
    }

    public void AddScoreToLeaderboard()
    {
        Social.ReportScore(Player.instance.score, "CgkI5MOyqIkYEAIQAA",(bool success)=> { });
    }

    public void ShowLeaderboard()
    {
        //Social.ShowLeaderboardUI();
        if (Social.localUser.authenticated)
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI5MOyqIkYEAIQAA");
        else
            LogIn();
    }

}
