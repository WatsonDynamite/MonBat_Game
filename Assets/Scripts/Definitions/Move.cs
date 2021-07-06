//this defines the structure of the moves that monsters use.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "MonBat_Game/Move", order = 0)]
public class Move : ScriptableObject
{
    //the name of a move
    public string name;
    //the move's description
    public string desc;
    //how much stamina the move costs to cast
    public int cost;
    //how much base power the move has
    public int power;
    //what type the move is
    public Type type; 
    //Category: physical, special, status, see: Category class
    public Category cat;
    //Priority: This shouldn't go lower than -4 or higher than 4 but there are no other constraints to it.
    public int priority;
    //array of special secondary effects triggered by the move
    public SecondaryEffect[] secondaryEffects;

    public Move(string nm, string dsc, int pow, int c, int pri, Type t, Category ct, SecondaryEffect[] scFX){
            name = nm;
            desc = dsc;
            cost = c;
            power = pow;
            type = t;
            cat = ct;
            priority = pri;
            secondaryEffects = scFX;
    }
}

