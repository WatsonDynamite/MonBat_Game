using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    //the name of a move
    public string name {get;} 
    //the move's description
    public string desc {get;}
    //how much stamina the move costs to cast
    public int cost {get;} 
    //how much base power the move has (to be used in dmg calculations)
    public int power {get;} 
    //what type the move is (for STAB calculation, effectiveness, etc.)
    public Type type {get;} 
    //Category: physical, special, status, see: Category class
    public Category cat {get;}
    //array of special secondary effects triggered by the move
    public SecondaryEffect[] secondaryEffects {get;} 

    public Move(string nm, string dsc, int pow, int c,  Type t, Category ct, SecondaryEffect[] scFX){
            name = nm;
            desc = dsc;
            cost = c;
            power = pow;
            type = t;
            cat = ct;
            secondaryEffects = scFX;
    }
}

public enum Category //whether the move uses sp.atk to hit sp.def or uses atk to hit def
{
    PHYSICAL,
    SPECIAL,
    STATUS
}


public enum SecondaryEffectType  //whether the effect affects the user or someone else
{
    SELF,
    OTHER
}
public enum SecondaryEffectEffect //the name of the effect proper
{   
    //HEALING
    HEALING_HALF, //50% HP recovery
    //BOOSTS
    BOOST_ATK_1, //+1 atk
    BOOST_DEF_1, //+1 def
    BOOST_SP_ATK_1, //you get the idea...
    BOOST_SP_DEF_1,
    BOOST_SPEED_1,
    //DROPS
    LOWER_ATK_1, //-1 atk
    LOWER_DEF_1, //-1 def
    LOWER_SP_ATK_1, //you get the idea...
    LOWER_SP_DEF_1,
    LOWER_SPEED_1
}

public class SecondaryEffect{ //secondary effects are a union of an effect (defense down, speed up, etc.) and the effect type (whether it affects the user or someone else)

    public SecondaryEffectEffect effect {get;}
    public SecondaryEffectType type {get;}

    public SecondaryEffect(SecondaryEffectType ty, SecondaryEffectEffect ef){
        effect = ef;
        type = ty;
    }

}

public static class SecondaryEffectList{  //used for global access of every secondary effect

    public static SecondaryEffect effectHeal {get;}
    public static SecondaryEffect effectDefDown {get;}
    public static SecondaryEffect effectSpeedUp {get;}

    static SecondaryEffectList(){
        effectHeal = new SecondaryEffect(SecondaryEffectType.SELF, SecondaryEffectEffect.HEALING_HALF); //Heals self for 50%
        effectDefDown = new SecondaryEffect(SecondaryEffectType.OTHER, SecondaryEffectEffect.LOWER_DEF_1); //Lowers enemy's defense by 1 stage
        effectSpeedUp = new SecondaryEffect(SecondaryEffectType.SELF, SecondaryEffectEffect.BOOST_SPEED_1); //Boosts own speed by 1 stage
    }
}

public class MoveExecutionAuxUnit{ //this is mostly used for doubles. Meant to be used when a move is invoked so that we can know who is the user, the move that was used, and the target of said move.
    public Monster user {get;}
    public Monster target {get;}
    public Move move {get;}

    public MoveExecutionAuxUnit(Monster usr, Monster trgt, Move mv){
        user = usr;
        target = trgt;
        move = mv;
    }

}

