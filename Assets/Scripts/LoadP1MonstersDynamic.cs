﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadP1MonstersDynamic : MonoBehaviour
{
    public CombatController combatController;

    //UI BUTTONS
    [Header("UI Buttons")]
    public GameObject SwitchButtonText;
    public GameObject MonBtnList;
    public GameObject MonBtn1;
    public GameObject MonBtn2;
    public GameObject MonBtn3;
    public GameObject MonBtn4;
    public GameObject MonBtn5;
    public GameObject MonBtn6;

    [Header("Tooltip Objects")]
    public GameObject MoveToolTip;


    private Sprite[] categsprites;
    private List<Monster> party;

    private List<GameObject> btnListAux;
    private int indAux = 0;

    void Awake()
    {
        btnListAux = new List<GameObject>() { MonBtn1, MonBtn2, MonBtn3, MonBtn4, MonBtn5, MonBtn6 };
    }

    // Start is called before the first frame update
    void Start()
    {
        
        LoadMonstersIntoUI();
    }

    public void LoadMonstersIntoUI()
    {

        //in here we get the monsters from the player's party and make the buttons match them.
        //We add a listener to each button
        party = combatController.getP1Party();
        Debug.Log(party[0].name);
        //MonBtnList.SetActive(false);
       
        foreach (GameObject button in btnListAux)
        {
            button.GetComponentInChildren<Text>().text = party[indAux].name;

            if (party[indAux].HP.value == 0 || party[indAux] == combatController.player1Monster)
            {
                button.GetComponentInChildren<Button>().interactable = false;
                if (party[indAux].HP.value == 0)
                {
                    button.GetComponentInChildren<Text>().text += "[FNT]";
                }
            }
            if(party[indAux] == MonsterList.monsterNone)
            {
                button.SetActive(false);
            }

            indAux++;
        }


        /*
        MonBtn1.GetComponentInChildren<Text>().text = party[0].name;
        //AtkBtn1.GetComponent<Button>().onClick.AddListener(delegate { turnQueuer(moveList[0]); });
        //AtkBtn1.GetComponent<Button>().onMouseOver.AddListener(delegate {ToolTipSnapToCursor();});

        AtkBtn2.GetComponentInChildren<Text>().text = party[1].name;
        AtkBtn2.GetComponentsInChildren<Image>()[1].sprite = TypeUtils.spriteByType(moveList[1].type);
        AtkBtn2.GetComponent<Button>().onClick.AddListener(delegate { turnQueuer(moveList[1]); });

        AtkBtn3.GetComponentInChildren<Text>().text = moveList[2].name;
        AtkBtn3.GetComponentsInChildren<Image>()[1].sprite = TypeUtils.spriteByType(moveList[2].type);
        AtkBtn3.GetComponent<Button>().onClick.AddListener(delegate { turnQueuer(moveList[2]); });

        AtkBtn4.GetComponentInChildren<Text>().text = moveList[3].name;
        AtkBtn4.GetComponentsInChildren<Image>()[1].sprite = TypeUtils.spriteByType(moveList[3].type);
        AtkBtn4.GetComponent<Button>().onClick.AddListener(delegate { turnQueuer(moveList[3]); });
        */
    }

    private void turnQueuer(Move playerMove)
    { //this gets both moves from each monster and begins the turn
        Debug.Log("Move selected: " + playerMove.name);
        List<Move> enemyMoveList = combatController.getP2Moves();
        Move enemyMove = enemyMoveList[Random.Range(0, enemyMoveList.Count - 1)];
        Debug.Log("Enemy move: " + enemyMove.name);
        StartCoroutine(combatController.ExecuteTurn(playerMove, enemyMove));
        ToggleMonsterList();

    }

    // Update is called once per frame
    void Update()
    {

    }

    /* 
    private Sprite spriteByType(Type type){ //returns the different symbol images for every type from the spritesheet. This will eventually be changed when the UI is made prettier
       return typesprites[(int) type]; //can't believe this works
    } 
    */




    public void ToggleMonsterList()
    { //this is behavior for the "Switch" button
        if (MonBtnList.activeSelf)
        {
            MonBtnList.SetActive(false);
            SwitchButtonText.GetComponentInChildren<Text>().text = "Switch";
        }
        else
        {
            MonBtnList.SetActive(true);
            SwitchButtonText.GetComponentInChildren<Text>().text = "Back";
        }
    }

    /*
    public void EnableMoveToolTip(GameObject btn)
    {
        Move move = MoveList.moveNone;
        ToolTipSnapToCursor();
        MoveToolTip.SetActive(true);
        if (btn == AtkBtn1)
        {
            move = moveList[0];
        }
        else if (btn == AtkBtn2)
        {
            move = moveList[1];
        }
        else if (btn == AtkBtn3)
        {
            move = moveList[2];
        }
        else if (btn == AtkBtn4)
        {
            move = moveList[3];
        }
        MoveToolTip.transform.Find("Txt_Name").GetComponent<Text>().text = move.name;
        MoveToolTip.transform.Find("Txt_Description").GetComponent<Text>().text = move.desc;
        MoveToolTip.transform.Find("Txt_Cost").GetComponent<Text>().text = move.cost.ToString();
        MoveToolTip.transform.Find("Txt_Power").GetComponent<Text>().text = move.power.ToString() == "0" ? "--" : move.power.ToString();
        MoveToolTip.transform.Find("Image").GetComponent<Image>().sprite = categsprites[(int)move.cat];
    }

    public void DisableMoveToolTip()
    {
        MoveToolTip.SetActive(false);
    }

    public void ToolTipSnapToCursor()
    {
        MoveToolTip.transform.position = new Vector3(Input.mousePosition.x + 6, Input.mousePosition.y + 6, Input.mousePosition.z);
    }
    */

}