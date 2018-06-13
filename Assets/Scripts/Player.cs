﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float maxHealth;
    
    private float lives;
    private bool alive;
    private float health;
    private Game game;
    private Character character;

    public Player(Character character)
    {
        this.character = character;
        health = character.GetHealth();
        game = Game.GetInstance();
        lives = game.GetLives();
        this.name = character.name;
    }

    public void TakeDamage(float damage)
    {
        float damageTake = character.CalculateDamage(damage);
        if(health - damageTake <= 0 )
        {
            notifyDeath();
        } else
        {
            health -= damageTake;
        }
    }
	
    public bool IsAlive()
    {
        return alive;
    }

    
    public void notifyDeath()
    {
        game.TriggerDeath(this);
    }
    
    // Update is called once per frame
	void Update () {
		
	}
}
