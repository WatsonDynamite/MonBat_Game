﻿//this defines the structure of the moves that monsters use.

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

[CreateAssetMenu(fileName = "Move", menuName = "MonBat_Game/SecondaryEffect", order = 0)]
///<summary>
 //secondary effects are a union of an effect (defense down, speed up, etc.) and the effect type (whether it affects the user or someone else)
///</summary>
public class SecondaryEffect : ScriptableObject {

    public SecondaryEffectEffect effect;
    public SecondaryEffectType type;

    public SecondaryEffect(string ty, string ef){
        effect = (SecondaryEffectEffect) Enum.Parse(typeof(SecondaryEffectEffect), ef);
        type = (SecondaryEffectType) Enum.Parse(typeof(SecondaryEffectType), ty);
    }

}

public static class SecondaryEffectList{  //used for global access of every secondary effect

    public static SecondaryEffect effectHeal {get;}
    public static SecondaryEffect effectDefDown {get;}
    public static SecondaryEffect effectSpeedUp {get;}
    public static SecondaryEffect poisonFive {get;}
    public static SecondaryEffect burnFive {get;}

    static SecondaryEffectList(){
        effectHeal = new SecondaryEffect("SELF", "HEALING_HALF"); //Heals self for 50%
        effectDefDown = new SecondaryEffect("OTHER", "LOWER_DEF_1"); //Lowers enemy's defense by 1 stage
        effectSpeedUp = new SecondaryEffect("SELF", "BOOST_SPEED_1"); //Boosts own speed by 1 stage
        poisonFive = new SecondaryEffect("OTHER", "POISON_FIVE"); //Poisons enemy for 5 turns
        burnFive = new SecondaryEffect("OTHER", "BURN_FIVE"); //Burns enemy for 5 turns
    }
}

public class MoveExecutionAuxUnit{ 
    //this will mostly be used for an eventual implementation of doubles. Meant to be used when a move is invoked so that we can know who is the user, the move that was used, and the target of said move.
    public Monster user {get;}
    public Monster target {get;}
    public Move move {get;}

    public MoveExecutionAuxUnit(Monster usr, Monster trgt, Move mv){
        user = usr;
        target = trgt;
        move = mv;
    }

}

