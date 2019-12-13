using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public enum Type
{
    NONE, //(for secondary types / error handling)
    WATER,
    FIRE,
    NATURE,
    ICE,
    ELECTRIC,
    TOXIC,
    SHADOW,
    MIND,
    LIGHT,
    MARTIAL,
    EARTH,
    METAL,
    WIND,
    ARCANE,
    
}
  
public class TypeUtils
{
//horizontal: attacking type
//vertical: defending type
   private static float[,] typeChart = new float[15, 15] {
/*NONE*/{/*NON*/1,  /*WTR*/1,   /*FIR*/1,   /*NTR*/1,   /*ICE*/1,   /*ELC*/1,   /*TOX*/1,   /*SHD*/1,   /*MND*/1,   /*LGT*/1,   /*MRT*/1,   /*ERT*/1,   /*MTL*/1,   /*WND*/1,   /*ARC*/1},
/*WATR*/{/*NON*/1,  /*WTR*/0.5f,/*FIR*/2,   /*NTR*/0.5f,/*ICE*/0.5f,/*ELC*/2,   /*TOX*/2,   /*SHD*/1,   /*MND*/1,   /*LGT*/1,   /*MRT*/1,   /*ERT*/2,   /*MTL*/1,   /*WND*/1,   /*ARC*/1},
/*FIRE*/{/*NON*/1,  /*WTR*/0.5f,/*FIR*/0.5f,/*NTR*/2,   /*ICE*/2,   /*ELC*/1,   /*TOX*/2,   /*SHD*/1,   /*MND*/1,   /*LGT*/1,   /*MRT*/1,   /*ERT*/1,   /*MTL*/2,   /*WND*/1,   /*ARC*/1},
/*NATR*/{/*NON*/1,  /*WTR*/2,   /*FIR*/0.5f,/*NTR*/0.5f,/*ICE*/1,   /*ELC*/1,   /*TOX*/0.5f,/*SHD*/1,   /*MND*/1,   /*LGT*/1,   /*MRT*/1,   /*ERT*/2,   /*MTL*/0.5f,/*WND*/0.5f,/*ARC*/1},
/*ICE */{/*NON*/1,  /*WTR*/0.5f,/*FIR*/2,   /*NTR*/2,   /*ICE*/0.5f,/*ELC*/1,   /*TOX*/1,   /*SHD*/1,   /*MND*/1,   /*LGT*/1,   /*MRT*/1,   /*ERT*/0,   /*MTL*/2,   /*WND*/2,   /*ARC*/1},
/*ELEC*/{/*NON*/1,  /*WTR*/2,   /*FIR*/1,   /*NTR*/0.5f,/*ICE*/1,   /*ELC*/0,   /*TOX*/1,   /*SHD*/1,   /*MND*/1,   /*LGT*/1,   /*MRT*/1,   /*ERT*/0,   /*MTL*/2,   /*WND*/2,   /*ARC*/1},
/*TOX */{/*NON*/1,  /*WTR*/2,   /*FIR*/1,   /*NTR*/2 ,  /*ICE*/1,   /*ELC*/1,   /*TOX*/0,   /*SHD*/1,   /*MND*/1,   /*LGT*/1,   /*MRT*/1,   /*ERT*/1,   /*MTL*/0,   /*WND*/1,   /*ARC*/1},
/*SHDW*/{/*NON*/1,  /*WTR*/1,   /*FIR*/1,   /*NTR*/1,   /*ICE*/1,   /*ELC*/1,   /*TOX*/0.5f,/*SHD*/0.5f,/*MND*/2,   /*LGT*/2,   /*MRT*/0.5f,/*ERT*/1,   /*MTL*/1,   /*WND*/1,   /*ARC*/0.5f},
/*MIND*/{/*NON*/1,  /*WTR*/1,   /*FIR*/1,   /*NTR*/1,   /*ICE*/1,   /*ELC*/1,   /*TOX*/2,   /*SHD*/0.5f,/*MND*/1,   /*LGT*/1,   /*MRT*/1,   /*ERT*/1,   /*MTL*/1,   /*WND*/1,   /*ARC*/1},
/*LIGH*/{/*NON*/1,  /*WTR*/1,   /*FIR*/0.5f,/*NTR*/1,   /*ICE*/2,   /*ELC*/1,   /*TOX*/1,   /*SHD*/2,   /*MND*/1,   /*LGT*/1,   /*MRT*/1,   /*ERT*/1,   /*MTL*/0.5f,/*WND*/1,   /*ARC*/0.5f},
/*MRTL*/{/*NON*/1,  /*WTR*/1,   /*FIR*/1,   /*NTR*/1,   /*ICE*/3,   /*ELC*/1,   /*TOX*/0.5f,/*SHD*/0,   /*MND*/0.5f,/*LGT*/0f,  /*MRT*/1,   /*ERT*/1,   /*MTL*/2,   /*WND*/0.5f,/*ARC*/1},
/*ERTH*/{/*NON*/1,  /*WTR*/2,   /*FIR*/1,   /*NTR*/1,   /*ICE*/2,   /*ELC*/2,   /*TOX*/1,   /*SHD*/1,   /*MND*/1,   /*LGT*/1,   /*MRT*/1,   /*ERT*/1,   /*MTL*/2,   /*WND*/0,   /*ARC*/1},
/*METL*/{/*NON*/1,  /*WTR*/0.5f,/*FIR*/0.5f,/*NTR*/1,   /*ICE*/2,   /*ELC*/0.5f,/*TOX*/1,   /*SHD*/1,   /*MND*/1,   /*LGT*/2,   /*MRT*/0.5f,/*ERT*/1,   /*MTL*/0.5f,/*WND*/1,   /*ARC*/2},
/*WIND*/{/*NON*/1,  /*WTR*/1,   /*FIR*/2,   /*NTR*/2,   /*ICE*/1,   /*ELC*/1,   /*TOX*/1,   /*SHD*/1,   /*MND*/1,   /*LGT*/1,   /*MRT*/0.5f,/*ERT*/1,   /*MTL*/2,   /*WND*/0.5f,/*ARC*/1},
/*ARCN*/{/*NON*/1,  /*WTR*/1,   /*FIR*/1,   /*NTR*/1,   /*ICE*/1,   /*ELC*/1,   /*TOX*/1,   /*SHD*/2,   /*MND*/1,   /*LGT*/0.5f,/*MRT*/1,   /*ERT*/1,   /*MTL*/0.5f,/*WND*/1,   /*ARC*/2}
}; 

public static float Effectiveness(Type attackType, Type defendType){
       return typeChart[(int) attackType, (int) defendType];
}
}
