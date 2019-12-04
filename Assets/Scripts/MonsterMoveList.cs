using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterList
{
   public static Monster testMon1;
   public static Monster testMon2;

   static MonsterList(){
        testMon1 = new Monster("TestMonster1", Type.FIRE, Type.NONE, 100, 100, 100, 100, 100, 100, MoveList.moveTest1, MoveList.moveTest2, MoveList.moveNone, MoveList.moveNone, Resources.Load<GameObject>("Jammo-Character/Prefabs/Jammo_LowPoly"));
        testMon2 = new Monster("TestMonster2", Type.WATER, Type.NONE, 100, 100, 100, 100, 100, 105, MoveList.moveTest2, MoveList.moveTest2, MoveList.moveNone, MoveList.moveNone, Resources.Load<GameObject>("Cube/Cube"));
   }
}

public static class MoveList
{
        public static Move moveTest1;
        public static Move moveTest2;
        public static Move moveNone;

    static MoveList(){
        moveNone = new Move("----", 0, 0, Type.NONE);
        moveTest1 = new Move("moveTest1", 50, 2, Type.FIRE);
        moveTest2 = new Move("moveTest2", 50, 2, Type.WATER);
    }    
}