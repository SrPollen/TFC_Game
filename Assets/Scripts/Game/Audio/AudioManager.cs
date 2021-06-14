using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        //si no hay audiosource lo iguala al acutal y si lo hay lo destruye
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        //Para que permanezca entre escenas
        DontDestroyOnLoad(gameObject);
        
        //por cada item en el array genera un componente audiosource con sus opciones
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    
    //Se le pasa el nombre audio y lo busca en array cuando lo encuentra procede a reproducirlo
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Play();
    }
    
    //Se le pasa el nombre audio y lo busca en arraycuando lo encuentra procede a pararlo
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Stop();
    }

}
