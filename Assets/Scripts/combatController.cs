using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class combatController : MonoBehaviour
{


    public GameObject player1Spawn;
    public GameObject player2Spawn;

    private Monster player1Monster;
    private Monster player2Monster;

    //this is the model that is loaded on the scene
    private GameObject player1MonsterInstance;
    private GameObject player2MonsterInstance;

    private List<Move> moveList;

    //name / health / stamina etc.
    [Header("HP / STA")]
    public GameObject P1MonsterName;
    public GameObject P2MonsterName;
    public GameObject P1HPText;
    public GameObject P1HPMeter;
    public GameObject P2HPText;
    public GameObject P2HPMeter;
    public GameObject MoveNamePlate;

    [Header("Attack Cams")]
    public GameObject P1Cam;
    public GameObject P2Cam;
    public GameObject cameraController;



  


    void Awake(){
        player1Monster = MonsterList.testMon1;
        player2Monster = MonsterList.testMon2;
    }


    // Start is called before the first frame update
    void Start()
    {
        //fill UI
        P1HPText.GetComponent<Text>().text = player1Monster.currentHP.ToString() + " / " + player1Monster.maxHP.ToString();
        P2HPText.GetComponent<Text>().text = player2Monster.currentHP.ToString() + " / " + player2Monster.maxHP.ToString();
        P1MonsterName.GetComponent<Text>().text = player1Monster.name;
        P2MonsterName.GetComponent<Text>().text = player2Monster.name;

        //spawn monsters
        player1MonsterInstance = Instantiate(player1Monster.model as GameObject, player1Spawn.transform.position, player1Spawn.transform.rotation);
        player1MonsterInstance.transform.SetParent(player1Spawn.transform);
        player2MonsterInstance = Instantiate(player2Monster.model as GameObject, player2Spawn.transform.position, player2Spawn.transform.rotation);
        player2MonsterInstance.transform.SetParent(player2Spawn.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExecuteTurn(Move playerMove, Move enemyMove){


        Debug.Log("Executing turn");
        //damage formula:
                // [(MovePower * 2) * STAB] * (Atk / Def) * 0.3 * modifiers


        // determine which moves to execute (params)
        // determine turn order
        List<Monster> attackQ = new List<Monster>();
        var speedDiff = player1Monster.SPEED.value * player1Monster.SPEED.statMod - player2Monster.SPEED.value * player1Monster.SPEED.statMod;
        //if positive, player 1 is first. If negative, player 2 is first. If 0, speed tie.

        if(speedDiff > 0){
            attackQ.Add(player1Monster);
            attackQ.Add(player2Monster);
        }else if(speedDiff < 0){
            attackQ.Add(player2Monster);
            attackQ.Add(player1Monster);
        }

        
        // for each attacker:
        double dmgP1atk = 0;
        double dmgP2atk = 0;
        
                foreach (Monster m in attackQ){
                         // determine move damage
                         bool isSTAB;

                         if(m == player1Monster){
                             if(player1Monster.type1 == playerMove.type || player1Monster.type2 == playerMove.type){
                                 //is stab
                                 isSTAB = true;
                             }else{ isSTAB = false; }
                             switch(playerMove.cat){
                                 case Category.PHYSICAL: dmgP1atk = ((playerMove.power * 2) * (isSTAB ? 1.5 : 1)) * (player1Monster.ATK.value / player2Monster.DEF.value) * 0.3 * (TypeUtils.Effectiveness(playerMove.type, player2Monster.type1) * TypeUtils.Effectiveness(playerMove.type, player2Monster.type2)); break;
                                 case Category.SPECIAL:  dmgP1atk = ((playerMove.power * 2) * (isSTAB ? 1.5 : 1)) * (player1Monster.spATK.value / player2Monster.spDEF.value) * 0.3 * (TypeUtils.Effectiveness(playerMove.type, player2Monster.type1) * TypeUtils.Effectiveness(playerMove.type, player2Monster.type2)); break;
                             
                            }
                             

                         }else if(m == player2Monster){
                            if(player2Monster.type2 == enemyMove.type || player2Monster.type2 == enemyMove.type){
                                 //is stab
                                 isSTAB = true;
                             }else{ isSTAB = false; }

                             switch(enemyMove.cat){
                                 case Category.PHYSICAL: dmgP2atk =  ((enemyMove.power * 2) * (isSTAB ? 1.5 : 1)) * (player2Monster.ATK.value / player1Monster.DEF.value) * 0.3 * (TypeUtils.Effectiveness(enemyMove.type, player1Monster.type1) * TypeUtils.Effectiveness(enemyMove.type, player1Monster.type2)); break;
                                 case Category.SPECIAL:  dmgP2atk =  ((enemyMove.power * 2) * (isSTAB ? 1.5 : 1)) * (player2Monster.spATK.value / player1Monster.spDEF.value) * 0.3 * (TypeUtils.Effectiveness(enemyMove.type, player1Monster.type1) * TypeUtils.Effectiveness(enemyMove.type, player1Monster.type2)); break;
                             
                            }

                         }
                }
               
        
                // execute move:
                    // play animation
                    // apply damage;
      player1Monster.receiveDamage((int) Mathf.Floor((float) dmgP2atk));
      player2Monster.receiveDamage((int) Mathf.Floor((float) dmgP1atk));


      P1HPText.GetComponent<Text>().text = player1Monster.currentHP.ToString() + " / " + player1Monster.maxHP.ToString();
      Debug.Log((float) player1Monster.currentHP / (float) player1Monster.maxHP);
      P2HPText.GetComponent<Text>().text = player2Monster.currentHP.ToString() + " / " + player2Monster.maxHP.ToString();


      //construct animation sequence based on events
      IEnumerator[] seq = new IEnumerator[]{};
      if(speedDiff > 0){
          seq = new IEnumerator[]{PlayAtkAnimP1(playerMove), HPBarDepleteAnim(P2HPMeter, (float) player2Monster.currentHP / (float) player2Monster.maxHP), PlayAtkAnimP2(enemyMove),  HPBarDepleteAnim(P2HPMeter, (float) player1Monster.currentHP / (float) player1Monster.maxHP), CameraStateRestore()};
      }else if(speedDiff < 0) {
          seq = new IEnumerator[]{PlayAtkAnimP2(enemyMove),  HPBarDepleteAnim(P1HPMeter, (float) player1Monster.currentHP / (float) player1Monster.maxHP), PlayAtkAnimP1(playerMove), HPBarDepleteAnim(P2HPMeter, (float) player2Monster.currentHP / (float) player2Monster.maxHP), CameraStateRestore()};
      }

      StartCoroutine(ExecuteTurnAnimations(seq));
        

    }


    public IEnumerator PlayAtkAnimP1(Move move){
       cameraController.SetActive(false);
       P1Cam.SetActive(true);
       MoveNamePlate.GetComponentInChildren<Text>().text = move.name;
       MoveNamePlate.SetActive(true);
       if((player1MonsterInstance).GetComponent<Animator>() != null){
            (player1MonsterInstance).GetComponent<Animator>().SetBool("attacking", true);
            StartCoroutine(AnimationStateRestore(player1MonsterInstance));
       }
       yield return new WaitForSeconds(2);
       P2Cam.SetActive(true);
       P1Cam.SetActive(false);
       yield return new WaitForSeconds(0.7f);
       MoveNamePlate.SetActive(false);
    }

    public IEnumerator PlayAtkAnimP2(Move move){
       cameraController.SetActive(false);
       P2Cam.SetActive(true);
       MoveNamePlate.GetComponentInChildren<Text>().text = move.name;
       MoveNamePlate.SetActive(true);
       if((player2MonsterInstance).GetComponent<Animator>() != null){
            (player2MonsterInstance).GetComponent<Animator>().SetBool("attacking", true);
            StartCoroutine(AnimationStateRestore(player2MonsterInstance));
       }
       yield return new WaitForSeconds(2);
       P1Cam.SetActive(true);
       P2Cam.SetActive(false);
       yield return new WaitForSeconds(0.7f);
       MoveNamePlate.SetActive(false);
    }


    IEnumerator HPBarDepleteAnim(GameObject img, float targetWidth){
        yield return new WaitForSeconds(0.4f);
        //P1HPMeter.GetComponent<Image>().rectTransform.localScale = new Vector3( (float) player1Monster.currentHP / (float) player1Monster.maxHP , 1, 1); 
        Debug.Log("Target Width: " + targetWidth);
        float aux = img.GetComponent<Image>().rectTransform.localScale.x;

        while(img.GetComponent<Image>().rectTransform.localScale.x > targetWidth){
            img.GetComponent<Image>().rectTransform.localScale = new Vector3(aux -= 0.08f, 1, 1);
            yield return new WaitForSeconds(0.014f);
            if(img.GetComponent<Image>().rectTransform.localScale.x < 0.5f){
                img.GetComponent<Image>().color = new Color32(252, 232, 0, 255);
            }
            if(img.GetComponent<Image>().rectTransform.localScale.x < 0.2f){
                img.GetComponent<Image>().color = new Color32(255, 0, 0, 255);

            }
        }

        yield return new WaitForSeconds(1);
    }

    public static IEnumerator ExecuteTurnAnimations(params IEnumerator[] sequence){
            for (int i = 0; i < sequence.Length; ++i){
                 while(sequence[i].MoveNext())
                        yield return sequence[i].Current;
            }
    }

    public IEnumerator CameraStateRestore(){
        P1Cam.SetActive(false);
            P2Cam.SetActive(false);
            cameraController.SetActive(true);
        yield return new WaitForSeconds(0);
    }



    //pause attack animation (testing)

    IEnumerator AnimationStateRestore(GameObject instance){
            yield return new WaitForSeconds(1.5f);
            instance.GetComponent<Animator>().SetBool("attacking", false);
    }


    public List<Move> getP1Moves(){
        if(player1Monster != null){
        return new List<Move>{player1Monster.move1, player1Monster.move2, player1Monster.move3, player1Monster.move4};
        }
            return null;
    }

    public List<Move> getP2Moves(){
        if(player1Monster != null){
        return new List<Move>{player2Monster.move1, player2Monster.move2, player2Monster.move3, player2Monster.move4};
        }
            return null;
    }

    




}

