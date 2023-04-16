using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] Text newGameText;
    [SerializeField] Text controlsText;
    [SerializeField] List<Text> controlsMenuText;
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject menuPanel;

    [SerializeField] Text highscoreText;
    [SerializeField] GameObject loggingButton;
    [SerializeField] GameObject spinner;
    // [SerializeField] GameObject allowDataButton;
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

    private void SetHighScoreText()
    {
        if (jSlib.IsRussian())
        {
            highscoreText.text = "лучший счёт: " + jSlib.ReturnHighScore();
        }
        else
        {
            highscoreText.text = "high score: " + jSlib.ReturnHighScore();
        }
    }

    private void SetLoginText()
    {
        if (jSlib.IsRussian())
        {
            loggingText.text = "привет, " + jSlib.Username();
        }
        else
        {
            loggingText.text = "hello, " + jSlib.Username();
        }
    }


    private void SetControlsMenuText()
    {
        if (jSlib.IsRussian())
        {
            controlsMenuText[0].text = "W, A, S, D - двигаться\n\nSPACE - рывок\n\nESC - пауза\n\nЛКМ для рисования заклинаний:";
            controlsMenuText[1].text = "- поле регенерации";
            controlsMenuText[2].text = "- удар молнии";
            controlsMenuText[3].text = "- замедляющая лужа";
            controlsMenuText[4].text = "- щит";
            controlsMenuText[5].text = "комбинируй заклинания:";
        }
        else
        {
            controlsMenuText[0].text = "W, A, S, D - to move\n\nSPACE - to dash\n\nESC - to pause\n\nto draw spell use left click:";
            controlsMenuText[1].text = "- healing meadow";
            controlsMenuText[2].text = "- lightning strike";
            controlsMenuText[3].text = "- slowing puddle";
            controlsMenuText[4].text = "- shield";
            controlsMenuText[5].text = "you can combine spells:";
        }
    }

    void Start()
    {
        // allowDataButton.SetActive(false);
        Time.timeScale = 1f;
        controlsPanel.SetActive(false);
        jSlib = FindObjectOfType<CallJSlib>();
        SetHighScoreText();
        spinner.SetActive(false);
        if (jSlib.IsLoggined())
        {
            loggingButton.SetActive(false);
            loggingText.gameObject.SetActive(true);
            SetLoginText();
            // if (jSlib.Username() == "player!")
            // {
            //     allowDataButton.SetActive(true);
            // }
        }
        else
        {
            jSlib.SendDataAfterAuthYandex();
            StartCoroutine("UpdateAfterLogging");
        }
        if (jSlib.IsRussian())
        {
            newGameText.text = "новая игра";
            controlsText.text = "управление";
            loggingButton.GetComponentInChildren<Text>().text = "вход";
        }
        else
        {
            newGameText.text = "new game";
            controlsText.text = "controls";
            loggingButton.GetComponentInChildren<Text>().text = "log in";
        }
        SetControlsMenuText();
    }

    public void OnAllowUserDataButton()
    {
        Debug.Log("Unity: trying to get user real name...");
        jSlib.AllowUserData();
        loggingText.gameObject.SetActive(false);
        spinner.SetActive(true);
        // allowDataButton.SetActive(false);
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
        SetLoginText();
        // if (jSlib.Username() == "player!")
        // {
        //     allowDataButton.SetActive(true);
        // }
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
                SetLoginText();
                SetHighScoreText();
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
        // if (jSlib.Username() == "player!")
        // {
        //     allowDataButton.SetActive(true);
        // }

        updateAfterLoggingStarted = false;
    }
}
