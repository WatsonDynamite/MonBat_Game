//this file defines the ScriptableObject template for the monsters in the game.
//it is essentially a "dumb" data structure. No functional purpose, it exists solely for the sake of data storage.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "MonBat_Game/Monster", order = 0)]
public class MonsterTemplate : ScriptableObject
{
    public new string name;
    public Type type1;
    public Type type2;

    //stats - I used pokemon names for ease of readability.
    public int HP;
    public int ATK;
    public int DEF;
    public int spATK;
    public int spDEF;
    public int SPEED;

    //moves - pretty self explanatory
    public Move move1;
    public Move move2;
    public Move move3;
    public Move move4;

    public GameObject prefab; //address of the monster to render.
}

