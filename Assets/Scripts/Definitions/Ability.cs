//this defines the structure of Monster abilities.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "MonBat_Game/Ability", order = 0)]
public class Ability : ScriptableObject
{  
    public string name;
    public string desc;
    public int turnCounter;
    public int periodicity;
    public AbilityFrequency frequency;
    public SecondaryEffect[] secondaryEffects;

    public Ability(string nm, string dsc, int prd, AbilityFrequency frq, SecondaryEffect[] fx) {
        name = nm;
        desc = dsc;
        turnCounter = 0;
        periodicity = prd;
        frequency = frq;
        secondaryEffects = fx;
    }

}