using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public string name {get;}
    public int power {get;}
    public int cost {get;}
    public Type type {get;}
    public Category cat {get;}  //for when we implement status moves

    public Move(string nm, int pow, int c, Type t, Category ct){
            name = nm;
            power = pow;
            cost = c;
            type = t;
            cat = ct;
    }
}


public enum Category
{
    PHYSICAL,
    SPECIAL,
    STATUS
}


