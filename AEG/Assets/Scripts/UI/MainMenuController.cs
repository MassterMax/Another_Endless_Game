using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] Text newGameText;
    [SerializeField] Button newGameButton;
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


    private void SetText()
    {
        Debug.Log("Unity: SetText() started");
        SetHighScoreText();
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
        Debug.Log("Unity: SetText() finished");
    }
    void Start()
    {
        // allowDataButton.SetActive(false);
        Time.timeScale = 1f;
        controlsPanel.SetActive(false);
        jSlib = FindObjectOfType<CallJSlib>();
        // SetText();
        StartCoroutine("SetLanguageUI");
        StartCoroutine("SetNewGameUI");
        StartCoroutine("SetLoginUI");
    }

    private void SetNotInitGameText()
    {
        newGameText.fontSize = 100;
        if (jSlib.IsRussian())
        {
            newGameText.text = "не получилось синхронизировать данные игрока, обновите страницу";
        }
        else
        {
            newGameText.text = "failed to get user data, please, refresh the page";
        }
    }

    public IEnumerator SetLanguageUI()
    {
        Debug.Log("Unity: SetLanguageUI start");
        SetText();
        int attempts = 8;
        while (attempts > 0)
        {
            Debug.Log("Unity: SetLanguageUI attempt " + (4 - attempts));
            attempts -= 1;
            if (jSlib.InitLang())
            {
                break;
            }
            else
            {
                yield return new WaitForSeconds(0.25f);
            }
        }
        SetText();
    }

    public IEnumerator SetNewGameUI()
    {
        newGameButton.interactable = false;
        newGameText.color = new Color(1, 1, 1, 0.5f);

        int attempts = 3;
        while (attempts > 0)
        {
            attempts -= 1;
            if (jSlib.playerInitialized)
            {
                break;
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
        // SetText();
        if (jSlib.playerInitialized)
        {
            newGameButton.interactable = true;
            newGameText.color = new Color(1, 1, 1, 1);
        }
        else
        {
            SetNotInitGameText();
        }
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
            if (!jSlib.playerInitialized)
            {
                loggingButton.SetActive(false);
            }
        }
        SetHighScoreText();
    }

}
