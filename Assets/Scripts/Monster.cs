using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    public string name {get;}
    
    public Type type1 {get;}
    public Type type2 {get;}

    public int maxHP {get; set;}
    public int currentHP{get; set;}

    public Stat HP {get;}
    public Stat ATK {get;}
    public Stat DEF {get;}
    public Stat spATK {get;}
    public Stat spDEF {get;}
    public Stat SPEED {get;}

    public Move move1 {get;}
    public Move move2 {get;}
    public Move move3 {get;}
    public Move move4 {get;}

    public Object model {get;}

    public Monster(string n, Type t1, Type t2, int hp, int atk, int def, int spatk, int spdef, int spd, Move m1, Move m2, Move m3, Move m4, Object mod){
        name = n;
        type1 = t1;
        type2 = t2;
        HP = new Stat(hp);
        currentHP = (int) Mathf.Round(hp * 1.5f);
        maxHP = currentHP;
        ATK = new Stat(atk);
        DEF = new Stat(def);
        spATK = new Stat (spatk);
        spDEF = new Stat (spdef);
        SPEED = new Stat(spd);
        move1 = m1;
        move2 = m2;
        move3 = m3;
        move4 = m4;
        model = mod;
    }

    public void receiveDamage(int dmg){
        currentHP = currentHP - dmg;
        if(currentHP < 0){
            currentHP = 0;
        }
    }

    public void healDamage(int dmg){
        currentHP = currentHP + dmg;
        if(currentHP > maxHP){
            currentHP = maxHP;
        }
    }

    public List<Move> getMoves(){
        List<Move> moves = new List<Move>();
        moves.Add(move1);
        moves.Add(move2);
        moves.Add(move3);
        moves.Add(move4);
        return moves;
    }
}

