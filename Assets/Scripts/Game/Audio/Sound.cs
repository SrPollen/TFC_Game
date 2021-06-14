using System;
using UnityEngine.Audio;
using UnityEngine;

//Clase sound la estructura de como se guardaran los audios
[Serializable]
public class Sound
{
    public string name;
    
    public AudioClip clip;

    [Range(0f,1f)]
    public float volume;
    
    [Range(.1f,3f)]
    public float pitch;

    public bool loop;
    
    [HideInInspector]
    public AudioSource source;
}
