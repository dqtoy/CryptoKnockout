﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRandomScript : MonoBehaviour {

    public GameObject laserSprite;
    public float delay = 1f;
    public float fireTime =  1f;
    public float targetRange = 20f;
    public Transform LaserSource;
    private float lastFire;
    private bool active;
    private Player[] players;
    private Transform currentTarget;
    Game game;
	// Use this for initialization
	void Start () {
        laserSprite.SetActive(false);
        lastFire = Time.time;
        game = Game.GetInstance();
	}
	
	// Update is called once per frame
	void Update () {
        //Check for game started
        if (!game)
        {
            game = Game.GetInstance();
            return;
        }

        //Get Players From Game
        UpdatePlayers();

        //Fire
        TriggerRandomFire();

	}

    void CheckForTargets()
    {
        List<Transform> viableTargets = new List<Transform>();
        //Check for Radius
        foreach (Player p in players)
        {
            if (Vector2.Distance(LaserSource.position, p.transform.position) <= targetRange)
            {
                viableTargets.Add(p.transform);
            }
        }
        if (viableTargets.Count >= 0)
        {
            currentTarget = viableTargets[Random.Range((int)0, (int)viableTargets.Count)];
        } else
        {
            currentTarget = null;
        }

    }

    void TriggerRandomFire()
    {

        if (active)
        {
            if (Time.time - lastFire >= fireTime + Random.Range(0.1f, 0.2f))
            {
                lastFire = Time.time;
                laserSprite.SetActive(false);
                active = false;
            }
        }
        else
        {
            if (game.GetState() != Game.State.FIGHTING) return;
            CheckForTargets();
            if (currentTarget)
            {
                if (Time.time - lastFire >= delay + Random.Range(1f, 10f))
                {
                    if (!(Random.Range(0, 100) > 80)) return;
                    lastFire = Time.time;
                    laserSprite.SetActive(true);
                    active = true;
                }
            }
        }
    }

    public Transform GetCurrentTarget()
    {
        return currentTarget;
    }

    //TODO needs to be optimized
    void UpdatePlayers()
    {
        if(players == null || players.Length == 0)
        {
            players = game.GetPlayers();
            return;
        }
       foreach(Player p in players)
        {
            if (!p)
            {
                players = game.GetPlayers();
                return;
            }
        }
    }
}
