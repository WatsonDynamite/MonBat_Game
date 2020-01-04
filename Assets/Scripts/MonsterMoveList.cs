using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterList
{
   public static Monster testMon1;
   public static Monster testMon2;

   static MonsterList(){
        //Monster construction: Name, Type 1, Type 2, HP, ATK, DEF, SP.ATK, SP.DEF, SPEED, Move 1, Move 2, Move 3, Move 4, Path to prefab
        testMon1 = new Monster("TestMonster1", Type.FIRE, Type.NONE, 100, 100, 100, 100, 100, 100, MoveList.fireball, MoveList.droplet, MoveList.prod, MoveList.recover, Resources.Load<GameObject>("Boximon_Fire/Boximon Fiery"));
        testMon2 = new Monster("TestMonster2", Type.WATER, Type.NONE, 100, 100, 100, 100, 100, 105, MoveList.fireball, MoveList.droplet, MoveList.prod, MoveList.rumble, Resources.Load<GameObject>("Boximon_Water/Boximon Cyclopes"));
   }
}

public static class MoveList
{
    public static Move moveNone;
    public static Move fireball;
    public static Move droplet;
    public static Move prod;

    public static Move rumble;
    public static Move shock;
    public static Move recover;
    public static Move intimidate;

    static MoveList(){
        //move construction: name, power, cost, type, category, side effects
        moveNone = new Move("----", 0, 0, Type.NONE, Category.STATUS, null);
        fireball = new Move("Fireball", 50, 2, Type.FIRE, Category.SPECIAL, null);
        droplet = new Move("Droplet", 50, 2, Type.WATER, Category.SPECIAL, null);
        prod = new Move("Prod", 330, 2, Type.NATURE, Category.PHYSICAL, null);
        rumble = new Move("Rumble", 50, 2, Type.EARTH, Category.PHYSICAL, null);
        shock = new Move("Shock", 50, 2, Type.ELECTRIC, Category.SPECIAL, null);
        recover = new Move("Recover", 0, 2, Type.LIGHT, Category.STATUS, new SecondaryEffect[] {SecondaryEffectList.effectHeal});
        intimidate = new Move("Intimidate", 0, 2, Type.SHADOW, Category.STATUS, new SecondaryEffect[]{SecondaryEffectList.effectDefDown});

    }    
}