﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character {

    protected float health = 100, special = 100, defence = 10, strength = 20, moveSpeed = 10, punchDamage, kickDamage, special1Damage, special2Damage, ultraDamage;
    protected string name;
    protected List<string> hitWords;
    protected Player player;

    /// <summary>
    /// This should be implemented to calculate damage taken based on characters special attributes, strength,
    /// defence, and a small bit of chance. 
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    abstract public float CalculateDamage(float damage);

    public void initializePlayer(Player player)
    {
        this.player = player;
        //Debug.Log("Player reference has been updated for " + name);
    }
    public float GetHealth()
    {
        return health;
    }

    public float GetDefence()
    {
        return defence;
    }

    public float GetStrength()
    {
        return strength;
    }
    
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public abstract void MovePunch();
    public abstract void MoveBlock();
    public abstract void MoveKick();
    public abstract void MoveSpecial1();
    public abstract void MoveSpecial2();
    public abstract void MoveUltra();

    public abstract AnimatorOverrideController GetAnimationController();


    public string GetName()
    {
        return name;
    }

    public float GetSpecial()
    {
        return special;
    }

    public List<string> GetHitWords()
    {
        return hitWords;
    }
    public enum Characters
    {
        BJORN,
        BITCOINBOY,
        DOGE,
        ETHBOT,
        MONERO
    }

    public static Character Get(Characters c)
    {
        switch (c)
        {
            case Characters.BJORN:
                return new Bjorn();
            case Characters.DOGE:
                return new Doge();
            case Characters.ETHBOT:
                return new EthBot();
            case Characters.BITCOINBOY:
                return new BTCBoy();
            default:
                return new Doge();
        }
    }
}
