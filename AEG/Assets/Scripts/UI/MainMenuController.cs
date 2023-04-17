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
        // if (!updateAfterLoggingStarted)
        // {
        jSlib.LogInYandex();
        StartCoroutine("SetLoginUI");
        // }
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
            highscoreText.text = "лучший счёт: " + jSlib.highScore;
        }
        else
        {
            highscoreText.text = "high score: " + jSlib.highScore;
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
            controlsMenuText[0].text = "W, A, S, D - двигаться\n\nSPACE - рывок\n\nЛКМ для рисования заклинаний:";
            controlsMenuText[1].text = "- поле регенерации";
            controlsMenuText[2].text = "- удар молнии";
            controlsMenuText[3].text = "- замедляющая лужа";
            controlsMenuText[4].text = "- щит";
            controlsMenuText[5].text = "комбинируй заклинания:";
        }
        else
        {
            controlsMenuText[0].text = "W, A, S, D - to move\n\nSPACE - to dash\n\nto draw a spell use left click:";
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
        // spinner.SetActive(false);
        // if (jSlib.IsLoggined())
        // {
        //     loggingButton.SetActive(false);
        //     loggingText.gameObject.SetActive(true);
        //     SetLoginText();
        // }
        // else
        // {
        //     jSlib.SendDataAfterAuthYandex();
        //     StartCoroutine("UpdateAfterLogging");
        // }
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
        StartCoroutine("SetLoginUI");
    }

    public IEnumerator SetLoginUI()
    {
        spinner.SetActive(true);
        loggingText.gameObject.SetActive(false);
        loggingButton.SetActive(false);

        int attempts = 3;
        while (attempts > 0)
        {
            attempts -= 1;
            if (jSlib.authorized)
            {
                break;
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }

        spinner.SetActive(false);
        if (jSlib.authorized)
        {
            loggingText.gameObject.SetActive(true);
            loggingButton.SetActive(false);
            SetLoginText();
        }
        else
        {
            loggingText.gameObject.SetActive(false);
            loggingButton.SetActive(true);
        }
        SetHighScoreText();
    }

    // public IEnumerator UpdateAfterLogging()
    // {
    //     updateAfterLoggingStarted = true;
    //     Debug.Log("Unity: try to UpdateAfterLogging");
    //     loggingText.gameObject.SetActive(false);
    //     loggingButton.SetActive(false);
    //     spinner.SetActive(true);
    //     int attempts = 4;
    //     while (attempts > 0)
    //     {
    //         attempts -= 1;
    //         if (jSlib.IsLoggined() && jSlib.IsHighScoreUpdated())
    //         {
    //             spinner.SetActive(false);
    //             loggingText.gameObject.SetActive(true);
    //             SetLoginText();
    //             SetHighScoreText();
    //             break;
    //         }
    //         else
    //         {
    //             yield return new WaitForSeconds(4 - attempts);
    //         }
    //     }

    //     if (!jSlib.IsLoggined() || !jSlib.IsHighScoreUpdated())
    //     {
    //         spinner.SetActive(false);
    //         loggingButton.SetActive(true);
    //     }
    //     // if (jSlib.Username() == "player!")
    //     // {
    //     //     allowDataButton.SetActive(true);
    //     // }

    //     updateAfterLoggingStarted = false;
    // }
}
