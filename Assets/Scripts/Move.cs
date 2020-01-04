using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public string name {get;}
    public int cost {get;}
    public int power {get;}
    public Type type {get;}
    public Category cat {get;}  //for when we implement status moves
    public SecondaryEffect[] secondaryEffects {get;}

    public Move(string nm, int pow, int c,  Type t, Category ct, SecondaryEffect[] scFX){
            name = nm;
            cost = c;
            power = pow;
            type = t;
            cat = ct;
            secondaryEffects = scFX;
    }
}

public enum Category
{
    PHYSICAL,
    SPECIAL,
    STATUS
}


public enum SecondaryEffectType
{
    SELF,
    OTHER
}
public enum SecondaryEffectEffect
{
    HEALING, //50% HP recovery
    BOOST_ATK, //+1 atk
    BOOST_SPEED, //+1 speed
    LOWER_DEF //-1 defense
}

public class SecondaryEffect{

    public SecondaryEffectEffect effect {get;}
    public SecondaryEffectType type {get;}

    public SecondaryEffect(SecondaryEffectType ty, SecondaryEffectEffect ef){
        effect = effect;
        type = type;
    }

}

public static class SecondaryEffectList{

    public static SecondaryEffect effectHeal {get;}
    public static SecondaryEffect effectDefDown {get;}

    static SecondaryEffectList(){
        effectHeal = new SecondaryEffect(SecondaryEffectType.SELF, SecondaryEffectEffect.HEALING);
        effectDefDown = new SecondaryEffect(SecondaryEffectType.OTHER, SecondaryEffectEffect.LOWER_DEF);
    }
}

