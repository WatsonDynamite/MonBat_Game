using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat {
    public int value;       //base value
    public int modValue;    //value with modifiers (same as base value if 0, except for HP)
    public int stage;       //buff stage
    public float statMod;   //value of modifier to apply

    public Stat(int val){
        value = val;
        stage = 0;
    }


    public bool buffStat(){
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

    public bool debuffStat(){
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
            return true;
        }else{
            Debug.Log("This stat cannot go any lower!");
            return false;
        }
    }

}

