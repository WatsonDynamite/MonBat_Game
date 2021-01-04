using System.Security.AccessControl;
using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MonsterList
{
   public static Monster monsterNone;

   static MonsterList(){
        //Monster construction: Name, Type 1, Type 2, HP, ATK, DEF, SP.ATK, SP.DEF, SPEED, Move 1, Move 2, Move 3, Move 4, Resources.Load<GameObject>("[Path to prefab]")
        monsterNone = new Monster("None", Type.NONE, Type.NONE, 0, 0, 0, 0, 0, 0, null, null, null, null, null); //this is for empty slots in parties and should never be visible to the player in normal gameplay
   }
}

public static class MoveList
{
    public static Move moveNone; //avoid using this
    public static Move delete; //this move is just a meme for testing, don't use

    static MoveList(){
        //move construction: name, power, cost, type, category, side effects
        //please make sure to not leave power at 0 for damaging moves.
        //it's ok if you mess up and assign a number to a status move's power, as it's never used.

        moveNone = new Move("----", "test move used for error handling", 0, 0, Type.NONE, Category.STATUS, null); //avoid using this, as a rule of thumb monsters should always have 4 moves
        delete = new Move("Delete", "Just straight up destroys the enemy fam", 4000, 0, Type.SHADOW, Category.SPECIAL, null); //this move is just a meme for testing, don't use
    }    
}