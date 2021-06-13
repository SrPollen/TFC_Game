using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    public AudioClip overSound, clickSound;
    private AudioSource _audioSource;
    private string gameScene = "PreGameScene";
    private string creditsScene = "Credits";
    private string loginScene = "LoginScene";
    

    private void Start()
    {
        _audioSource = GameObject.Find("AudioController")?.GetComponent<AudioSource>();
    }

    public void CustomMouseEnter()
    {
        _audioSource.PlayOneShot(overSound, 1);
    }

    public void ExitAction()
    {
        Application.Quit();
    }

    public void PlayAction()
    {
        Debug.Log("Play");
        SceneManager.LoadScene(gameScene);
    }

    public void CreditAction()
    {
        Debug.Log("Credits");
        SceneManager.LoadScene(creditsScene);
    }

    public void LogOutAction()
    {
        Debug.Log("LogOut");
        SceneManager.LoadScene(loginScene);
    }
}

