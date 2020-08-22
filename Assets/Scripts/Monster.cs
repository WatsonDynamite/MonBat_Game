//this file defines the data structure of the monsters of the game, including stats, moves, types, etc.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    public string name {get;} //name of the monster that is displayed in-game.
    
    public Type type1 {get;} //first type. Always fill.
    public Type type2 {get;} //secondary type. Use Type.NONE if none. NEVER LEAVE AS 'NULL', IT WILL BREAK THINGS

    public int maxHP {get; set;} //max health value (used for healing calculations and to display to the user). DO NOT CONFUSE WITH THE HP STAT ITSELF, BECAUSE I SURE DID.
    public int currentHP{get; set;} //the current HP value. Think of this variable as the actual "life bar".

    public StatusEffect statusEffect {get; set;} //status effect that the monster is currently afflicted with.

    //stats - I used pokemon names for ease of readability.
    public Stat HP {get;}
    public Stat ATK {get;}
    public Stat DEF {get;}
    public Stat spATK {get;}
    public Stat spDEF {get;}
    public Stat SPEED {get;}

    //moves - pretty self explanatory
    public Move move1 {get;}
    public Move move2 {get;}
    public Move move3 {get;}
    public Move move4 {get;}

    public Object model {get;} //Model that the monster will render. This is a Unity Prefab. Read the comment in the Constructor for more info.

    public Monster(string n, Type t1, Type t2, int hp, int atk, int def, int spatk, int spdef, int spd, Move m1, Move m2, Move m3, Move m4, Object mod){
        name = n;
        type1 = t1;
        type2 = (t2 == null? Type.NONE : t2); //if the secondary type was specified as null, it is changed to the NONE default type. I SAID NO NULLs
        HP = new Stat(hp);
        maxHP = (int) Mathf.Round(hp * 1.5f); //MAX HP is always the base stat * 1.5. it is the only stat where this happens.
        currentHP = maxHP; //every monster starts at full health
        ATK = new Stat(atk);
        DEF = new Stat(def);
        spATK = new Stat (spatk);
        spDEF = new Stat (spdef);
        SPEED = new Stat(spd);
        move1 = m1;
        move2 = m2;
        move3 = m3;
        move4 = m4;
        model = mod; //this should be a Prefab put in the Resources folder, loaded by Resources.Load(). See the MonsterMoveList file
        statusEffect = null; //this definitely should not stay "null"
    }

    /*this constructor makes it possible to "clone" the static monsters from the monster list into non-static instances. This is very important for gameplay.
    let me tell you a story of a time before this constructor existed:
    I actually used the static monster variable in combat and a very funny thing happened:
    if both players had the same monster, whatever happened to one monster would happen to the other.
    my dumba** forgot that since the monster values are static, they all point to the same data.
    so I created this constructor that copies the value of a given monster into a separate variable so that this doesn't happen. */
    public Monster(Monster mon){ 
        name = mon.name;
        type1 = mon.type1;
        type2 = mon.type2;
        HP = mon.HP;
        currentHP = mon.currentHP; 
        maxHP = mon.maxHP;
        ATK = mon.ATK;
        DEF = mon.DEF;
        spATK = mon.spATK;
        spDEF = mon.spDEF;
        SPEED = mon.SPEED;
        move1 = mon.move1;
        move2 = mon.move2;
        move3 = mon.move3;
        move4 = mon.move4;
        model = mon.model;
        statusEffect = null;
    }


    public void receiveDamage(int dmg){ //Lowers the current HP of this monster by the amount specified by DMG.
        currentHP = currentHP - dmg;
        if(currentHP < 0){
            currentHP = 0;
        }
    }

    public void healDamage(int dmg){  //Raises the current HP of this monster by the amount specified by DMG.
        currentHP = currentHP + dmg;
        if(currentHP > maxHP){
            currentHP = maxHP;
        }
    }

    public List<Move> getMoves(){ //returns the list of moves known by this monster.
        List<Move> moves = new List<Move>();
        moves.Add(move1);
        moves.Add(move2);
        moves.Add(move3);
        moves.Add(move4);
        return moves;
    }

    public void ApplyStatusEffect(StatusEffectEnum type, int turns){ //applies a status effect to this monster.

        statusEffect = new StatusEffect(type, turns);

    }

    //compares 2 monsters, returns true if they are identical.
    //I have no idea what I use this for
    public bool Compare(Monster mon){
        return(type1 == mon.type1 && type2 == mon.type2 && HP == mon.HP && ATK == mon.ATK && DEF == mon.DEF && spATK == mon.spATK && spDEF == mon.spDEF && SPEED == mon.SPEED && move1 == mon.move1 && move2 == mon.move2 && move3 == mon.move3 && move4 == mon.move4 && model == mon.model);
    }
}

