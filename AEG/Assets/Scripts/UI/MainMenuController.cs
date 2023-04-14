using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject menuPanel;

    [SerializeField] Text highscoreText;
    [SerializeField] GameObject loggingButton;
    [SerializeField] GameObject spinner;
    [SerializeField] GameObject allowDataButton;
    [SerializeField] Text loggingText;
    bool updateAfterLoggingStarted = false;

    CallJSlib jSlib;

    public void OnNewGameButton()
    {
        Debug.Log("new game");
        SceneManager.LoadScene(1);
    }

    public void OnLogInButton()
    {
        if (!updateAfterLoggingStarted)
        {
            jSlib.LogInYandex();
            StartCoroutine("UpdateAfterLogging");
        }
    }


    public void OnControlsButton()
    {
        menuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void OnOkButton()
    {
        menuPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    void Start()
    {
        allowDataButton.SetActive(false);
        Time.timeScale = 1f;
        controlsPanel.SetActive(false);
        jSlib = FindObjectOfType<CallJSlib>();
        highscoreText.text = "HIGH SCORE: " + jSlib.ReturnHighScore();
        spinner.SetActive(false);
        if (jSlib.IsLoggined())
        {
            loggingButton.SetActive(false);
            loggingText.gameObject.SetActive(true);
            loggingText.text = "hello, " + jSlib.Username();
            if (jSlib.Username() == "player!")
            {
                allowDataButton.SetActive(true);
            }
        }
        else
        {
            jSlib.SendDataAfterAuthYandex();
            StartCoroutine("UpdateAfterLogging");
        }

    }

    public void OnAllowUserDataButton()
    {
        Debug.Log("Unity: trying to get user real name...");
        jSlib.AllowUserData();
        loggingText.gameObject.SetActive(false);
        spinner.SetActive(true);
        allowDataButton.SetActive(false);
        StartCoroutine("AfterAllowUserDataButton");
    }

    public IEnumerator AfterAllowUserDataButton()
    {
        float timer = 10f;
        while (timer > 0)
        {
            timer -= 1f;
            yield return new WaitForSeconds(1);
            if (jSlib.Username() != "player!")
            {
                break;
            }
        }
        loggingText.gameObject.SetActive(true);
        spinner.SetActive(false);
        loggingText.text = "hello, " + jSlib.Username();
        if (jSlib.Username() == "player!")
        {
            allowDataButton.SetActive(true);
        }
    }

    public IEnumerator UpdateAfterLogging()
    {
        updateAfterLoggingStarted = true;
        Debug.Log("Unity: try to UpdateAfterLogging");
        loggingText.gameObject.SetActive(false);
        loggingButton.SetActive(false);
        spinner.SetActive(true);
        int attempts = 4;
        while (attempts > 0)
        {
            attempts -= 1;
            if (jSlib.IsLoggined() && jSlib.IsHighScoreUpdated())
            {
                spinner.SetActive(false);
                loggingText.gameObject.SetActive(true);
                loggingText.text = "hello, " + jSlib.Username();
                highscoreText.text = "HIGH SCORE: " + jSlib.ReturnHighScore();
                break;
            }
            else
            {
                yield return new WaitForSeconds(4 - attempts);
            }
        }

        if (!jSlib.IsLoggined() || !jSlib.IsHighScoreUpdated())
        {
            spinner.SetActive(false);
            loggingButton.SetActive(true);
        }
        if (jSlib.Username() == "player!")
        {
            allowDataButton.SetActive(true);
        }

        updateAfterLoggingStarted = false;
    }
}
