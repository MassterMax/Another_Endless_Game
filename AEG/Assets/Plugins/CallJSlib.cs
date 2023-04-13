using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CallJSlib : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Hello();


    [DllImport("__Internal")]
    private static extern void SetToLeaderboard(int value);


    [DllImport("__Internal")]
    private static extern void InitLb();

    [DllImport("__Internal")]
    private static extern void LogIn();

    [DllImport("__Internal")]
    private static extern void GetHighscore();

    [DllImport("__Internal")]
    private static extern void GetUsername();

    public static CallJSlib SingletonInstance { get; private set; }

    private bool loggined = false;
    private int highScore = 0;
    private string username = "";


    public int ReturnHighScore()
    {
        return highScore;
    }

    public void UpdateUsername(string value)
    {
        username = value;
    }

    public string Username() { return username; }

    public bool IsLoggined() { return loggined; }

    public void UpdateHighScore(int value)
    {
        highScore = value;
    }

    void Awake()
    {
        if (SingletonInstance != null && SingletonInstance != this)
        {
            Destroy(this);
        }
        else
        {
            SingletonInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void HelloButton()
    {
        Hello();
    }

    // // call this from js after logging
    // public void SetLeaderboardHighscore(int value)
    // {
    //     highScore = Mathf.Max(highScore, value);
    // }

    public void SetToYandexleaderboard(int value)
    {
        highScore = Mathf.Max(highScore, value);
        try
        {
            if (loggined)
                SetToLeaderboard(value);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void LogInYandex()
    {
        if (loggined) return;
        try
        {
            LogIn();
            InitLb();
            Debug.Log("sucessfully logged in");
            loggined = true;
            // try to update user leaderboard hs with current value
            GetUsername();
            GetHighscore();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}
