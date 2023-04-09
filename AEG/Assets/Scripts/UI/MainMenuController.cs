using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void OnNewGameButton()
    {
        Debug.Log("new game");
        SceneManager.LoadScene(1);
    }


    public void OnControlsButton()
    {
        Debug.Log("controls");
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
