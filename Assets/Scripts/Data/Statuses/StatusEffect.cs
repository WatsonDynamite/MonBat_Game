//this file defines the structure for the status effects.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusEffect
{
    public StatusEffectEnum statusEffectType {get;} //this defines the type that this status effect will be.
    public int maxTurns;                            //this defines the number of turns that this status effect will be active. This isn't defined by the type in order to give more freedon.
    public int turnCounter;                         //counts the number of turns that this status effect has been active. Obviously the idea here is that once this number equals maxTurns, the effect fades.

    public StatusEffect(StatusEffectEnum type, int tc){
        statusEffectType = type;
        maxTurns = tc;
        turnCounter = 0;
    }


    public void IncrementStatusCounter(){
        turnCounter++;
    }

}


public enum StatusEffectEnum
{
    POISONED,
    BURNED,
}

public class StatusUtils 
{
    private static Sprite[] statusinds = Resources.LoadAll<Sprite>("UISprites/statusinds"); 

    public static Sprite spriteByType(StatusEffectEnum status){
        Debug.Log(statusinds.Length);
        Debug.Log(status);
        Debug.Log((int) status);
        //returns the different symbol images for every type from the spritesheet. This will eventually be changed when the UI is made prettier
        return statusinds[(int) status]; //can't believe this works
    } 
}
