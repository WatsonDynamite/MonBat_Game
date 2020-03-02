using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterList
{
   public static Monster testMon1;
   public static Monster testMon2;
   public static Monster monsterNone;

   static MonsterList(){

        //Monster construction: Name, Type 1, Type 2, HP, ATK, DEF, SP.ATK, SP.DEF, SPEED, Move 1, Move 2, Move 3, Move 4, Resources.Load<GameObject>("[Path to prefab]")
        monsterNone = new Monster("None", Type.NONE, Type.NONE, 0, 0, 0, 0, 0, 0, MoveList.moveNone, MoveList.moveNone, MoveList.moveNone, MoveList.moveNone, null); //this is for empty slots in parties and should never be visible to the player in normal gameplay
        testMon1 = new Monster("TestMonster1", Type.FIRE, Type.NONE, 100, 100, 100, 100, 100, 100, MoveList.fireball, MoveList.prod, MoveList.recover, MoveList.intimidate, Resources.Load<GameObject>("Boximon_Fire/Boximon Fiery"));
        testMon2 = new Monster("TestMonster2", Type.WATER, Type.NONE, 100, 100, 100, 100, 100, 105, MoveList.shock, MoveList.rumble, MoveList.droplet, MoveList.sabotage, Resources.Load<GameObject>("Boximon_Water/Boximon Cyclopes"));

   }
}

public static class MoveList
{
    public static Move moveNone; //avoid using this
    public static Move delete; //this move is just a meme for testing, don't use

    public static Move fireball;
    public static Move droplet;
    public static Move prod;
    public static Move rumble;
    public static Move shock;
    public static Move recover;
    public static Move intimidate;
    public static Move sabotage;

    static MoveList(){
        //move construction: name, power, cost, type, category, side effects
        //please make sure to not leave power at 0 for damaging moves.
        //it's ok if you mess up and assign a number to a status move's power, as it's never used.

        moveNone = new Move("----", "test move used for error handling", 0, 0, Type.NONE, Category.STATUS, null); //avoid using this, as a rule of thumb monsters should always have 4 moves
        delete = new Move("Delete", "Just straight up destroys the enemy fam", 4000, 0, Type.SHADOW, Category.SPECIAL, null);//this move is just a meme for testing, don't use


        fireball = new Move("Fireball", "The enemy is singed with a small fireball.", 50, 2, Type.FIRE, Category.PHYSICAL, null);
        droplet = new Move("Droplet", "The enemy is soaked with a small droplet.", 50, 2, Type.WATER, Category.SPECIAL, null);
        prod = new Move("Prod", "The enemy is prodded with a small branch.", 50, 2, Type.NATURE, Category.PHYSICAL, null);
        rumble = new Move("Rumble", "The user makes the ground rumble slightly, tripping the opponent.", 50, 2, Type.EARTH, Category.PHYSICAL, null);
        shock = new Move("Shock", "The enemy is lightly shocked by electrity.", 50, 2, Type.ELECTRIC, Category.SPECIAL, null);
        recover = new Move("Recover", "The user bathes in light, healing 50% of its health.", 0, 2, Type.LIGHT, Category.STATUS, new SecondaryEffect[] {SecondaryEffectList.effectHeal});
        intimidate = new Move("Intimidate", "The user intimidates the opponent, lowering their Attack stat by one stage.", 0, 2, Type.SHADOW, Category.STATUS, new SecondaryEffect[]{SecondaryEffectList.effectDefDown});
        sabotage = new Move("Sabotage", "The user strikes the opponent, raising their own speed and lowering the enemy's defense.", 10, 2, Type.MARTIAL, Category.PHYSICAL, new SecondaryEffect[]{SecondaryEffectList.effectDefDown, SecondaryEffectList.effectSpeedUp});
       
    }    
}