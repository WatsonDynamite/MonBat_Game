using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterList
{
   public static Monster testMon1;
   public static Monster testMon2;

   static MonsterList(){
        testMon1 = new Monster("TestMonster1", Type.FIRE, Type.NONE, 100, 100, 100, 100, 100, 100, MoveList.moveTest1, MoveList.moveTest2, MoveList.moveTest3, MoveList.moveTest4, Resources.Load<GameObject>("Jammo-Character/Prefabs/Jammo_LowPoly"));
        testMon2 = new Monster("TestMonster2", Type.WATER, Type.NONE, 100, 100, 100, 100, 100, 105, MoveList.moveTest2, MoveList.moveTest2, MoveList.moveTest3, MoveList.moveTest4, Resources.Load<GameObject>("Jammo-Character/Prefabs/Jammo_LowPoly2"));
   }
}

public static class MoveList
{
        public static Move moveTest1;
        public static Move moveTest2;
        public static Move moveTest3;
        public static Move moveTest4;
        public static Move moveNone;

    static MoveList(){
        moveNone = new Move("----", 0, 0, Type.NONE, Category.STATUS);
        moveTest1 = new Move("Fireball", 50, 2, Type.FIRE, Category.SPECIAL);
        moveTest2 = new Move("Droplet", 50, 2, Type.WATER, Category.SPECIAL);
        moveTest3 = new Move("Prod", 50, 2, Type.NATURE, Category.PHYSICAL);
        moveTest4 = new Move("Rumble", 50, 2, Type.EARTH, Category.PHYSICAL);

    }    
}