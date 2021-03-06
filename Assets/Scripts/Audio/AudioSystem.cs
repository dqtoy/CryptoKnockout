﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour {

    public static HashSet<AudioSource> sources = new HashSet<AudioSource>();


    public static void Register(AudioSource source)
    {
        sources.Add(source);
        source.volume = GlobalGameData.GetInstance().volume;
    }

    public void UpdateAudio(float volume)
    {
        foreach(AudioSource source in sources){
            try
            {
                source.volume = volume;
            }
            catch
            {
                Debug.Log("Audio Source No Longer Available");
                sources.Remove(source);
            }
        }
    }

    public static void Play(AudioSource audioSource, AudioClip clip)
    {
        Play(audioSource, clip, false);
    }

    public static void Play(AudioSource audioSource, AudioClip clip, bool loop)
    {
        if (!audioSource || !clip) return;
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.time = 0;
        audioSource.loop = loop;
        audioSource.Play();
    }







    // Use this for initialization
    void Start () {
        		
	}

    void Awake()
    {
    }
	
	// Update is called once per frame
	void Update () {
		
	}


}
