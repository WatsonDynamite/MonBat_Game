using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    private string name {get;}
    private int power {get;}
    private int cost {get;}
    private Type type {get;}
    //private Category cat   //for when we implement status moves

    public Move(string name, int pow, int c, Type t){
            power = pow;
            cost = c;
            type = t;
    }
}



public enum Category
{
    OFFENSIVE,
    STATUS
}