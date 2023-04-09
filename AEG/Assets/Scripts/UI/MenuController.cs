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

    private static bool reflectionsTurnedOn;

    public static bool ReflectionsTurnedOn { get => reflectionsTurnedOn; }

    private bool paused;
    private bool ended = false;

    private void Start()
    {
        OnReflectionOffButton();
        paused = false;
        SwitchPauseMenu(false);
        totalScorePanel.SetActive(false);
    }

    private void Update()
    {
        if (!ended && Input.GetKeyDown(KeyCode.Escape))
        {
            OnPauseButton();
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

        pauseMenuText.text = "GAME OVER!\n-";
        reflectionPanel.SetActive(false);
        totalScorePanel.SetActive(true);
        totalScorePanel.GetComponentInChildren<Text>().text = "TOTAL SCORE IS " + totalScore;
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
}
