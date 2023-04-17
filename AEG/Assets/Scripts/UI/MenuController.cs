using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject reflectionOffButton, reflectionOnButton;
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] Text pauseMenuText;
    [SerializeField] GameObject totalScorePanel;
    [SerializeField] GameObject reflectionPanel;

    [SerializeField] List<Text> pauseMenuTexts;

    [SerializeField] GameObject pauseButton;

    CallJSlib jSlib;
    private static bool reflectionsTurnedOn;

    public static bool ReflectionsTurnedOn { get => reflectionsTurnedOn; }

    private bool paused;
    private bool ended = false;

    private bool isRussian;
    private void Start()
    {
        jSlib = FindObjectOfType<CallJSlib>();
        OnReflectionOffButton();
        paused = false;
        SwitchPauseMenu(false);
        totalScorePanel.SetActive(false);
        isRussian = jSlib.IsRussian();
        SetPauseMenuTexts();
    }

    private void Update()
    {
        // if (!ended && Input.GetKeyDown(KeyCode.Escape))
        // {
        //     OnPauseButton();
        // }
    }

    private void SetPauseMenuTexts()
    {
        if (isRussian)
        {
            pauseMenuTexts[0].text = "ПАУЗА";
            pauseMenuTexts[1].text = "отражения";
            pauseMenuTexts[2].text = "выход";
            pauseMenuTexts[3].text = "нажми для продолжения";
        }
        else
        {
            pauseMenuTexts[0].text = "PAUSED";
            pauseMenuTexts[1].text = "reflections";
            pauseMenuTexts[2].text = "exit";
            pauseMenuTexts[3].text = "press to resume";
        }
    }

    public void OnPauseButton()
    {
        paused = !paused;
        SwitchPauseMenu(paused);
    }

    public void OnDeath(int totalScore)
    {
        ended = true;
        Debug.Log("total score is " + totalScore);
        paused = true;
        SwitchPauseMenu(paused);

        if (isRussian)
        {
            pauseMenuText.text = "ИГРА ОКОНЧЕНА\n\n-";
            totalScorePanel.GetComponentInChildren<Text>().text = "итоговый счёт: " + totalScore;
        }
        else
        {
            pauseMenuText.text = "GAME OVER\n\n-";
            totalScorePanel.GetComponentInChildren<Text>().text = "total score is " + totalScore;
        }

        reflectionPanel.SetActive(false);
        totalScorePanel.SetActive(true);
        pauseMenuTexts[3].gameObject.SetActive(false);
    }

    public void OnMenuButton()
    {
        Debug.Log("bye!");
        SceneManager.LoadScene(0);
    }

    private void SwitchPauseMenu(bool turnOn)
    {
        Time.timeScale = turnOn ? 0f : 1f;
        pauseMenuPanel?.SetActive(turnOn);
        pauseButton.SetActive(!turnOn);
    }

    // we press on reflection [ON] button and it switches to off button -> reflections turn off
    public void OnReflectionOnButton()
    {
        Debug.Log("clicked reflection on button");
        reflectionOnButton.SetActive(false);
        reflectionOffButton.SetActive(true);
        TurnReflections(false);
        reflectionsTurnedOn = false;
    }

    public void OnReflectionOffButton()
    {
        Debug.Log("clicked reflection off button");
        reflectionOnButton.SetActive(true);
        reflectionOffButton.SetActive(false);
        TurnReflections(true);
        reflectionsTurnedOn = true;
    }

    private void TurnReflections(bool on)
    {
        foreach (var reflecatble in FindObjectsOfType<Reflectable>())
        {
            reflecatble.Turn(on);
        }

        foreach (var reflecting in FindObjectsOfType<Reflecting>())
        {
            reflecting.Turn(on);
        }
    }

    void OnApplicationPause(bool pause)
    {
        if (pause && !paused)
            OnPauseButton();
    }
}
