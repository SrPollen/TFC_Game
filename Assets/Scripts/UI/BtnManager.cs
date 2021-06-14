using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    private string gameScene = "PreGameScene";
    private string creditsScene = "Credits";
    private string loginScene = "LoginScene";

    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("MainTheme");
    }

    public void CustomMouseEnter()
    {
        FindObjectOfType<AudioManager>().Play("HoverButton");
    }

    public void ExitAction()
    {
        FindObjectOfType<AudioManager>().Play("ClickButton");
        Application.Quit();
    }

    public void PlayAction()
    {
        FindObjectOfType<AudioManager>().Play("ClickButton");
        SceneManager.LoadScene(gameScene);
    }

    public void CreditAction()
    {
        FindObjectOfType<AudioManager>().Play("ClickButton");
        FindObjectOfType<AudioManager>().Stop("MainTheme");
        SceneManager.LoadScene(creditsScene);
    }

    public void LogOutAction()
    {
        FindObjectOfType<AudioManager>().Play("ClickButton");
        SceneManager.LoadScene(loginScene);
    }
}

