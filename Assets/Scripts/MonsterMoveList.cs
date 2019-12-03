using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterList
{
   public static Monster testMon1;

   static MonsterList(){
        testMon1 = new Monster("TestMonster1", Type.FIRE, Type.NONE, 100, 100, 100, 100, 100, 100, MoveList.moveTest1, MoveList.moveTest2, null, null, Resources.Load("Jammo-Character/Prefabs/Jammo_LowPoly.prefab"));
   }
}

public static class MoveList
{
        public static Move moveTest1;
        public static Move moveTest2;

    static MoveList(){
        moveTest1 = new Move("moveTest1", 50, 2, Type.FIRE);
        moveTest2 = new Move("moveTest2", 50, 2, Type.WATER);
    }    
}