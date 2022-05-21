using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject reflectionOffButton, reflectionOnButton;
    [SerializeField] GameObject pauseMenuPanel;

    private static bool reflectionsTurnedOn;

    public static bool ReflectionsTurnedOn { get => reflectionsTurnedOn; }

    private bool paused;

    private void Start()
    {
        OnReflectionOffButton();
        paused = false;
        SwitchPauseMenu(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPauseButton();
        }
    }

    public void OnPauseButton()
    {
        paused = !paused;
        SwitchPauseMenu(paused);
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
