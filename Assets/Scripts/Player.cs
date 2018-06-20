﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    [SyncVar]
    private float lives = 1;
    [SyncVar]
    private bool alive = false;
    [SyncVar]
    public float health = 0;
    private Game game;
    private Character character;

    void Start()
    {
        this.character = new TestCharacter();
        health = character.GetHealth();
        game = Game.GetInstance();
        lives = game.GetLives();
        this.name = character.GetName();
        game.RegisterPlayer(this);
    }


    //To be done only by server
    public void TakeDamage(float damage)
    {
        if (!isServer) return;
        
        Debug.Log("Taking Damage");
        float damageTake = character.CalculateDamage(damage);
        if(health - damageTake <= 0 )
        {
            notifyDeath();
        } else
        {
            health -= damageTake;
        }
    }
    public void onHealthChange()
    {

    }
	
    public bool IsAlive()
    {
        return alive;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMoveSpeed()
    {
       return character.GetMoveSpeed();
    }
    public void notifyDeath()
    {
        game.TriggerDeath(this);
    }
    
    // Update is called once per frame
	void Update () {
		
	}
}
