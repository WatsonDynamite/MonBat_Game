using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadP1MovesDynamic : MonoBehaviour
{
    public combatController combatController;

    //UI BUTTONS
     [Header("UI Buttons")]
    public GameObject FightButtonText;
    public GameObject AtkBtnList;
    public GameObject AtkBtn1;
    public GameObject AtkBtn2;
    public GameObject AtkBtn3;
    public GameObject AtkBtn4;

    // Start is called before the first frame update
    void Start()
    {
        var moveList = combatController.getP1Moves();
        AtkBtnList.SetActive(false);
        AtkBtn1.GetComponentInChildren<Text>().text = moveList[0].name + " (" + moveList[0].type + ")";
            AtkBtn1.GetComponent<Image>().color = colorByType(moveList[0].type);
           AtkBtn1.GetComponent<Button>().onClick.AddListener(delegate {turnQueuer(moveList[0]);});

        AtkBtn2.GetComponentInChildren<Text>().text = moveList[1].name + " (" + moveList[1].type + ")";
            AtkBtn2.GetComponent<Image>().color = colorByType(moveList[1].type);
            AtkBtn2.GetComponent<Button>().onClick.AddListener(delegate {turnQueuer(moveList[1]);});

        AtkBtn3.GetComponentInChildren<Text>().text = moveList[2].name + " (" + moveList[2].type + ")";;
            AtkBtn3.GetComponent<Image>().color = colorByType(moveList[2].type);
            AtkBtn3.GetComponent<Button>().onClick.AddListener(delegate {turnQueuer(moveList[2]);});

        AtkBtn4.GetComponentInChildren<Text>().text = moveList[3].name + " (" + moveList[3].type + ")";;
            AtkBtn4.GetComponent<Image>().color = colorByType(moveList[3].type);
            AtkBtn4.GetComponent<Button>().onClick.AddListener(delegate {turnQueuer(moveList[3]);});
    }

    private void turnQueuer(Move playerMove){
        Debug.Log("Move selected: " + playerMove.name);
        List<Move> enemyMoveList = combatController.getP2Moves();
        Move enemyMove = enemyMoveList[Random.Range(0, enemyMoveList.Count - 1)];
        Debug.Log("Enemy move: " + enemyMove.name);
        combatController.ExecuteTurn(playerMove, enemyMove);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Color colorByType(Type type){
        Color temp = new Color32(0, 0, 0, 255);
        switch(type){
            case Type.NONE: temp = new Color32(0, 0, 0, 255); break;
            case Type.WATER: temp = new Color32(74, 134, 232, 255); break;
            case Type.FIRE: temp = new Color32(255, 0, 0, 255); break;
            case Type.NATURE: temp = new Color32(0, 255, 0, 255); break;
            case Type.ICE: temp = new Color32(0, 255, 255, 255); break;
            case Type.ELECTRIC: temp = new Color32(255, 255, 0, 255); break;
            case Type.TOXIC: temp = new Color32(153, 0, 255, 255); break;
            case Type.SHADOW: temp = new Color32(32, 18, 77, 255); break;
            case Type.MIND: temp = new Color32(255, 0, 255, 255); break;
            case Type.LIGHT: temp = new Color32(255, 229, 153, 255); break;
            case Type.MARTIAL: temp = new Color32(166, 28, 0, 255); break;
            case Type.EARTH: temp = new Color32(133, 32, 12, 255); break;
            case Type.METAL: temp = new Color32(137, 137, 137, 255); break;
            case Type.WIND: temp = new Color32(164, 194, 244, 255); break;
            case Type.ARCANE: temp = new Color32(246, 178, 107, 255); break;
        }
        return temp;
    } 

    public void ToggleMoveList(){
        if(AtkBtnList.activeSelf){
            AtkBtnList.SetActive(false);
            FightButtonText.GetComponentInChildren<Text>().text = "Fight";
        }else{
            AtkBtnList.SetActive(true);
            FightButtonText.GetComponentInChildren<Text>().text = "Back";
        }
    }
}
