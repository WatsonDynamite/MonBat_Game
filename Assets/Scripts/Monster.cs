using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    public string name {get;}
    
    public Type type1 {get;}
    public Type type2 {get;}

    public Stat HP {get;}
    public Stat ATK {get;}
    public Stat DEF {get;}
    public Stat spATK {get;}
    public Stat spDEF {get;}
    public Stat SPD {get;}

    public Move move1 {get;}
    public Move move2 {get;}
    public Move move3 {get;}
    public Move move4 {get;}

    public Object model {get;}

    public Monster(string n, Type t1, Type t2, int hp, int atk, int def, int spatk, int spdef, int spd, Move m1, Move m2, Move m3, Move m4, Object mod){
        name = n;
        type1 = t1;
        type2 = t2;
        HP = new Stat((int) Mathf.Round(hp * 1.5f));
        ATK = new Stat(atk);
        DEF = new Stat(def);
        spATK = new Stat (spatk);
        spDEF = new Stat (spdef);
        SPD = new Stat(spd);
        move1 = m1;
        move2 = m2;
        move3 = m3;
        move4 = m4;
        model = mod;
    }
}

