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
    [SerializeField] Text loggingText;

    CallJSlib jSlib;

    public void OnNewGameButton()
    {
        Debug.Log("new game");
        SceneManager.LoadScene(1);
    }

    public void OnLogInButton()
    {
        jSlib.LogInYandex();
        highscoreText.text = "HIGH SCORE: " + jSlib.ReturnHighScore();
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
        Time.timeScale = 1f;
        controlsPanel.SetActive(false);
        jSlib = FindObjectOfType<CallJSlib>();
        highscoreText.text = "HIGH SCORE: " + jSlib.ReturnHighScore();
        if (jSlib.IsLoggined())
        {
            loggingButton.SetActive(false);
            loggingText.gameObject.SetActive(true);
            loggingText.text = "hello, " + jSlib.Username();
        }
        else
        {
            loggingButton.SetActive(true);
            loggingText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
