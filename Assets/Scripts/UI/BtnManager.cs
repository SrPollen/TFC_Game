using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BtnManager : MonoBehaviour
{
    public AudioClip overSound, clickSound;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GameObject.Find("AudioController")?.GetComponent<AudioSource>();
    }

    public void CustomMouseEnter()
    {
        _audioSource.PlayOneShot(overSound, 1);
    }

}
