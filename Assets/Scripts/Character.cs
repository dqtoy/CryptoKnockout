﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character {

    protected float health, defence, strength, moveSpeed;
    protected string name;

    /// <summary>
    /// This should be implemented to calculate damage taken based on characters special attributes, strength,
    /// defence, and a small bit of chance. 
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    abstract public float CalculateDamage(float damage);

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

    public string GetName()
    {
        return name;
    }

}
