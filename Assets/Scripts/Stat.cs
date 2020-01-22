using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat {
    public int value;       //base value
    public int stage;       //buff stage
    public float statMod;   //value of modifier to apply

    public Stat(int val){
        value = val;
        statMod = 1;
        stage = 0;
    }

    public float getTrueValue(){ //used to easily access a stat after modifiers. ALWAYS USE THIS FOR DAMAGE CALCULATION unless the move is supposed to ignore modifiers.
        return value * statMod;
    }


    public bool buffStat(){ //boosts a stat one stage. I may have to allow this to receive an int for the number of stages intended to boost.
        if(stage < 6){
            stage++;
            switch (stage){
                case 6:     statMod = 4; break;
                case 5:     statMod = 3.5f; break;
                case 4:     statMod = 3; break;
                case 3:     statMod = 2.5f; break;
                case 2:     statMod = 2; break;
                case 1:     statMod = 1.5f; break;
                case 0:     statMod = 1; break;
                case -1:    statMod = 0.66f; break;
                case -2:    statMod = 0.5f; break;
                case -3:    statMod = 0.4f; break;
                case -4:    statMod = 0.33f; break;
                case -5:    statMod = 0.28f; break;
                case -6:    statMod = 0.25f; break;
            }
            return true;
        }else{
            Debug.Log("This stat cannot go any higher!");
            return false;
        }
    }

    public bool debuffStat(){ //same as buffstat, but it lowes the stage instead of boosting it.
        if(stage > -6){
            stage--;
            switch (stage){
                case 6:     statMod = 4; break;
                case 5:     statMod = 3.5f; break;
                case 4:     statMod = 3; break;
                case 3:     statMod = 2.5f; break;
                case 2:     statMod = 2; break;
                case 1:     statMod = 1.5f; break;
                case 0:     statMod = 1; break;
                case -1:    statMod = 0.66f; break;
                case -2:    statMod = 0.5f; break;
                case -3:    statMod = 0.4f; break;
                case -4:    statMod = 0.33f; break;
                case -5:    statMod = 0.28f; break;
                case -6:    statMod = 0.25f; break;
            }
            Debug.Log("Stage: " + stage);
            Debug.Log("Modifier: " + statMod);
            return true;
        }else{
            Debug.Log("This stat cannot go any lower!");
            return false;
        }
    }

}

