using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private DontDestroyOnLoad ddol;

    void Awake()
    {
        if ( instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.playOnAwake = false;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }
    }

    private void Update()
    {
        foreach (Sound s in sounds)
        {
            if(s.name == "Music")
            {
                s.source.volume = s.volume * ddol.mainVolume/50 * ddol.musicVolume/50;
            }
            else
            {
                s.source.volume = s.volume * ddol.mainVolume/50 * ddol.sfxVolume/50;
            }
        }
    }

    private void Start()
    {
        Play("Music");
        ddol = FindObjectOfType<DontDestroyOnLoad>();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!" + " or other error");
            return;
        }
            
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!" + " or other error");
            return;
        }

        s.source.Stop();
    }
}
