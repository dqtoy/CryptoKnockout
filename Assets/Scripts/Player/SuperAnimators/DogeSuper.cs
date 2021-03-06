﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogeSuper : SuperAnimationControl {

    Transform target;
    public AudioSource audioSource;

    public AudioClip laserWarmUp;
    public AudioClip laserFire;
    public AudioClip chainFall;

    public GameObject hitObject;

    public override void Start()
    {

        base.Start();
    }

    public override void StartSequence()
    {
        PlayAudio.Play(audioSource, laserWarmUp);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        Debug.Log("Just Here");
        base.StartSequence();
    }

    public override void UpdateIntro()
    {
        Debug.Log("I'm Here");
        if (Time.time >= midTime)
        {
            hitObject.transform.position = opponent.transform.position;
            AudioSystem.Play(audioSource, laserFire, true);
            animationObject.SetActive(true);
            NextSequence();
        }
    }

    public override void UpdateMid()
    {
        if (Time.time >= postTime)
        {
            AudioSystem.Play(audioSource, chainFall);
            NextSequence();
        }
    }

    public override void UpdatePost()
    {
        if (Time.time >= endTime)
        {
            animationObject.SetActive(false);
            player.NotifySuperComplete();
            NextSequence();
        }
    }

    public override void UpdateEnd()
    {
        base.UpdateEnd();
    }


}
