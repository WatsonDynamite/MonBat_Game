using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    MOVE,
    SWITCH,
    REST
}

public class TurnAction
{
    public ActionType actionType{get;}
    public Move move{get;}
    public Monster monster{get;}

    public TurnAction(Move mv){
            actionType = ActionType.MOVE;
            move = mv;
    }

    public TurnAction(ActionType at){
            actionType = at;
            move = MoveList.moveNone;
    }

    public TurnAction(Monster mn){
            actionType = ActionType.SWITCH;
            move = MoveList.moveNone;
            monster = mn;
    }
}
