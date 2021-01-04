public enum Category //whether the move uses sp.atk to hit sp.def or uses atk to hit def
{
    PHYSICAL,
    SPECIAL,
    STATUS
}


public enum SecondaryEffectType  //whether the secondary effect of the move applies to the user or the target
{
    SELF,
    OTHER
}

public enum SecondaryEffectEffect //the name of the effect proper. This doesn't actually -do- anything, the actual effect is applied on CombatController.
{   
    //HEALING
    HEALING_HALF, //50% HP recovery
    //BOOSTS
    BOOST_ATK_1, //+1 atk
    BOOST_DEF_1, //+1 def
    BOOST_SP_ATK_1, //you get the idea...
    BOOST_SP_DEF_1,
    BOOST_SPEED_1,
    //DROPS
    LOWER_ATK_1, //-1 atk
    LOWER_DEF_1, //-1 def
    LOWER_SP_ATK_1, //you get the idea...
    LOWER_SP_DEF_1,
    LOWER_SPEED_1,
    //STATUSES
    POISON_FIVE //poisons for 5 turns
}