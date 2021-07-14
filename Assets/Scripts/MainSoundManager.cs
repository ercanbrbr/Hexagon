using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainSoundManager : MonoBehaviour
{
    private static MainSoundManager instance;
    float volume = 0.5f;

    public Sound[] sounds;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        Play("background");
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        if (s != null)
        {
            if (s.source.isPlaying)
                return;
            s.source.Play();
        }
    }
    void Start()
    {
        volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        Play("Theme");
    }
}
