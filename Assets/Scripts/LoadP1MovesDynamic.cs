using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadP1MovesDynamic : MonoBehaviour
{
    public CombatController combatController;

    //UI BUTTONS
     [Header("UI Buttons")]
    public GameObject FightButtonText;
    public GameObject AtkBtnList;
    public GameObject AtkBtn1;
    public GameObject AtkBtn2;
    public GameObject AtkBtn3;
    public GameObject AtkBtn4;

    [Header("Tooltip Objects")]
    public GameObject MoveToolTip;

    
    private Sprite[] categsprites;
    private List<Move> moveList;

    void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start() 
    {  
       moveList = combatController.getP1Moves();
       categsprites = Resources.LoadAll<Sprite>("UISprites/PhysSpecStaInd");
       LoadMovesIntoUI();
    }

    public void LoadMovesIntoUI(){
         //in here we get the moves from the player's current active monster and make the buttons match them.
         //We add a listener to each button

        AtkBtnList.SetActive(false);
        AtkBtn1.GetComponentInChildren<Text>().text = moveList[0].name;
           AtkBtn1.GetComponentsInChildren<Image>()[1].sprite = TypeUtils.spriteByType(moveList[0].type);
           AtkBtn1.GetComponent<Button>().onClick.AddListener(delegate {turnQueuer(moveList[0]);});
           //AtkBtn1.GetComponent<Button>().onMouseOver.AddListener(delegate {ToolTipSnapToCursor();});

        AtkBtn2.GetComponentInChildren<Text>().text = moveList[1].name;
            AtkBtn2.GetComponentsInChildren<Image>()[1].sprite = TypeUtils.spriteByType(moveList[1].type);
            AtkBtn2.GetComponent<Button>().onClick.AddListener(delegate {turnQueuer(moveList[1]);});

        AtkBtn3.GetComponentInChildren<Text>().text = moveList[2].name;
            AtkBtn3.GetComponentsInChildren<Image>()[1].sprite = TypeUtils.spriteByType(moveList[2].type);
            AtkBtn3.GetComponent<Button>().onClick.AddListener(delegate {turnQueuer(moveList[2]);});

        AtkBtn4.GetComponentInChildren<Text>().text = moveList[3].name;
            AtkBtn4.GetComponentsInChildren<Image>()[1].sprite = TypeUtils.spriteByType(moveList[3].type);
            AtkBtn4.GetComponent<Button>().onClick.AddListener(delegate {turnQueuer(moveList[3]);});
    }

    private void turnQueuer(Move playerMove){ //this gets both moves from each monster and begins the turn
        Debug.Log("Move selected: " + playerMove.name);
        List<Move> enemyMoveList = combatController.getP2Moves();
        Move enemyMove = enemyMoveList[Random.Range(0, enemyMoveList.Count - 1)];
        Debug.Log("Enemy move: " + enemyMove.name);
        StartCoroutine(combatController.ExecuteTurn(playerMove, enemyMove));
        ToggleMoveList();
        
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




    private Color colorByType(Type type){ //returns different colors for every type. This will eventually be changed when the UI is made prettier
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

    public void ToggleMoveList(){ //this is behavior for the "Fight" button
        if(AtkBtnList.activeSelf){
            AtkBtnList.SetActive(false);
            FightButtonText.GetComponentInChildren<Text>().text = "Fight";
        }else{
            AtkBtnList.SetActive(true);
            FightButtonText.GetComponentInChildren<Text>().text = "Back";
        }
    }

    public void EnableMoveToolTip(GameObject btn){
        Move move = MoveList.moveNone;
        ToolTipSnapToCursor();
        MoveToolTip.SetActive(true);
            if(btn == AtkBtn1){
                    move = moveList[0];
            }
            else if(btn == AtkBtn2){
                    move = moveList[1];
            }
            else if(btn == AtkBtn3){
                    move = moveList[2];
            }
            else if(btn == AtkBtn4){
                    move = moveList[3];
            }
        MoveToolTip.transform.Find("Txt_Name").GetComponent<Text>().text = move.name;
        MoveToolTip.transform.Find("Txt_Description").GetComponent<Text>().text = move.desc;
        MoveToolTip.transform.Find("Txt_Cost").GetComponent<Text>().text = move.cost.ToString();
        MoveToolTip.transform.Find("Txt_Power").GetComponent<Text>().text = move.power.ToString() == "0"? "--": move.power.ToString();
        MoveToolTip.transform.Find("Image").GetComponent<Image>().sprite = categsprites[(int) move.cat];
    }

    public void DisableMoveToolTip(){
        MoveToolTip.SetActive(false);
    }

    public void ToolTipSnapToCursor(){
        MoveToolTip.transform.position = new Vector3(Input.mousePosition.x + 6, Input.mousePosition.y + 6, Input.mousePosition.z);
    }

}
