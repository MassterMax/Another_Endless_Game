using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject menuPanel;

    public void OnNewGameButton()
    {
        Debug.Log("new game");
        SceneManager.LoadScene(1);
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
