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


    public enum Actions{
        ATTACK,
        SWITCH,
        
        FAINT
    }

    delegate IEnumerator AtkAnim(Move move);
    delegate IEnumerator FaintAnim();
    delegate IEnumerator BuffAnim(Move move);
    delegate IEnumerator DebuffAnim(Move move);
  


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


    public void ExecuteTurn(Move player1Move, Move player2Move){
         List<IEnumerator> seq = new List<IEnumerator>();
         // determine turn order
        var speedDiff = player1Monster.SPEED.value * player1Monster.SPEED.statMod - player2Monster.SPEED.value * player1Monster.SPEED.statMod;
        //if positive, player 1 is first. If negative, player 2 is first. If 0, speed tie.

        if(speedDiff > 0){
           StartCoroutine(DoMoves(player1Move, player2Move, player1Monster, player2Monster));
        }else if(speedDiff < 0){
           StartCoroutine(DoMoves(player2Move, player1Move, player2Monster, player1Monster));
        }
    
    }


    public IEnumerator DoMoves(Move firstMove, Move secondMove, Monster first, Monster second){
        var isSTAB = false;
        var dmg = 0;
        AtkAnim AtkAnimDel1st;
        FaintAnim FaintAnimDel1st;
        BuffAnim BuffAnimDel1st;

        AtkAnim AtkAnimDel2nd;
        FaintAnim FaintAnimDel2nd;
        BuffAnim BuffAnimDel2nd;

        if(first == player1Monster){
            AtkAnimDel1st = new AtkAnim(PlayAtkAnimP1);
            FaintAnimDel1st = new FaintAnim(PlayFaintAnimP1);
            BuffAnimDel1st = new BuffAnim(PlayBuffAnimP1);
            

            AtkAnimDel2nd = new AtkAnim(PlayAtkAnimP2);
            FaintAnimDel2nd = new FaintAnim(PlayFaintAnimP2);
            BuffAnimDel2nd = new BuffAnim(PlayBuffAnimP2);
            

        }else{
            
            AtkAnimDel1st = new AtkAnim(PlayAtkAnimP2);
            FaintAnimDel1st = new FaintAnim(PlayFaintAnimP2);
            

            AtkAnimDel2nd = new AtkAnim(PlayAtkAnimP1);
            FaintAnimDel2nd = new FaintAnim(PlayFaintAnimP1);
            BuffAnimDel2nd = new BuffAnim(PlayBuffAnimP1);
        }


            if(first.type1 == firstMove.type || first.type2 == firstMove.type){
                //is stab
                isSTAB = true;
            }else{
                isSTAB = false;
            }

            //FIRST ATTACKS SECOND
            switch(firstMove.cat){
             case Category.PHYSICAL: var dmg1 = (int) Mathf.Floor((float) ((firstMove.power) * (isSTAB ? 1.5f : 1f)) * (first.ATK.value / second.DEF.value) * 0.3f * (TypeUtils.Effectiveness(firstMove.type, second.type1) * TypeUtils.Effectiveness(firstMove.type, second.type2)));
                                     Debug.Log("First attacks second: " + dmg1);
                                     second.receiveDamage(dmg1);
                                     yield return AtkAnimDel1st(firstMove);
                                     break;

             case Category.SPECIAL:  var dmg2 = (int) Mathf.Floor((float) ((firstMove.power) * (isSTAB ? 1.5f : 1f)) * (first.spATK.value / second.spDEF.value) * 0.3f * (TypeUtils.Effectiveness(firstMove.type, second.type1) * TypeUtils.Effectiveness(firstMove.type, second.type2)));
                                     Debug.Log("First attacks second: " + dmg2);
                                     second.receiveDamage(dmg2);
                                     yield return AtkAnimDel1st(firstMove);
                                     break;

                          
                             
            }

            if(second.currentHP <= 0){
                                            yield return FaintAnimDel2nd();
                                             //turn is over
                                     }



            if(second.type1 == secondMove.type || second.type2 == secondMove.type){
                //is stab
                isSTAB = true;
            }else{
                isSTAB = false;
            }
            //SECOND ATTACKS FIRST
            switch(secondMove.cat){
             case Category.PHYSICAL: var dmg3 = (int) Mathf.Floor(((secondMove.power) * (isSTAB ? 1.5f : 1f)) * (second.ATK.value / first.DEF.value) * 0.3f * (TypeUtils.Effectiveness(secondMove.type, first.type1) * TypeUtils.Effectiveness(secondMove.type, first.type2)));
                                     first.receiveDamage(dmg3);
                                     Debug.Log("Second attacks first: " + dmg3);
                                     yield return AtkAnimDel2nd(secondMove);
                                     break;

             case Category.SPECIAL:  first.receiveDamage((int) Mathf.Floor(((secondMove.power) * (isSTAB ? 1.5f : 1f)) * (second.spATK.value / first.spDEF.value) * 0.3f * (TypeUtils.Effectiveness(secondMove.type, first.type1) * TypeUtils.Effectiveness(secondMove.type, first.type2))));
                                     yield return AtkAnimDel2nd(secondMove);
                                     break;

             case Category.STATUS:  var affectsOthers = false;
                                    foreach (SecondaryEffect se in secondMove.secondaryEffects){
                                                     if(se.type == SecondaryEffectType.OTHER){
                                                         affectsOthers = true;
                                                     }                              
                                    }

                                    if(affectsOthers){
                                        
                                    }else{
                                        yield return BuffAnimDel2nd(secondMove);
                                        foreach (SecondaryEffect se in secondMove.secondaryEffects){
                                                                                    ApplyEffect(se.effect, second);
                                                                                }
                                    }
                                    
                                    break;  

            }      
                             
            

            if(first.currentHP <= 0){
                                            yield return FaintAnimDel1st();
                                            yield break; //turn is over
                                     }



    }


    public void ApplyEffect(SecondaryEffectEffect effect, Monster monster){
        switch (effect){
                case SecondaryEffectEffect.HEALING:
                Debug.Log("HEAL: " + (int)(monster.maxHP / 2f));
                        monster.healDamage((int)(monster.maxHP / 2f));
                        if(monster == player1Monster){
                            StartCoroutine(HPBarFillAnim(P1HPMeter, (float) player1Monster.currentHP / (float) player1Monster.maxHP));
                            P1HPText.GetComponent<Text>().text = player1Monster.currentHP.ToString() + " / " + player1Monster.maxHP.ToString();
                        }else{
                            StartCoroutine(HPBarFillAnim(P2HPMeter, (float) player2Monster.currentHP / (float) player2Monster.maxHP));
                            P2HPText.GetComponent<Text>().text = player2Monster.currentHP.ToString() + " / " + player2Monster.maxHP.ToString();
                        }
                        break;
        }
    }


    public IEnumerator PlayAtkAnimP1(Move move){
       cameraController.SetActive(false);
       P1Cam.SetActive(true);
       MoveNamePlate.GetComponentInChildren<Text>().text = move.name;
       MoveNamePlate.SetActive(true);
       if((player1MonsterInstance).GetComponent<Animator>() != null){
            (player1MonsterInstance).GetComponent<Animator>().SetBool("Attack 01", true);
            StartCoroutine(AnimationStateRestore(player1MonsterInstance));
       }
       yield return new WaitForSeconds(2);
       P2Cam.SetActive(true);
       P1Cam.SetActive(false);
       if((player2MonsterInstance).GetComponent<Animator>() != null){
            (player2MonsterInstance).GetComponent<Animator>().SetBool("Take Damage", true);
            StartCoroutine(HPBarDepleteAnim(P2HPMeter, (float) player2Monster.currentHP / (float) player2Monster.maxHP));
            P2HPText.GetComponent<Text>().text = player2Monster.currentHP.ToString() + " / " + player2Monster.maxHP.ToString();
            StartCoroutine(AnimationStateRestore(player2MonsterInstance));
       }
       yield return new WaitForSeconds(0.7f);
       MoveNamePlate.SetActive(false);
       StartCoroutine(CameraStateRestore());
    }


    public IEnumerator PlayFaintAnimP1(){
       cameraController.SetActive(false);
       P1Cam.SetActive(true);
       if((player1MonsterInstance).GetComponent<Animator>() != null){
            (player1MonsterInstance).GetComponent<Animator>().SetBool("Die", true);
       }
       yield return new WaitForSeconds(1);
       Destroy(player1MonsterInstance);
    }

    public IEnumerator PlayBuffAnimP1(Move move){
       cameraController.SetActive(false);
       P1Cam.SetActive(true);
       MoveNamePlate.GetComponentInChildren<Text>().text = move.name;
       MoveNamePlate.SetActive(true);
       if((player1MonsterInstance).GetComponent<Animator>() != null){
            (player1MonsterInstance).GetComponent<Animator>().SetBool("Attack 02", true);
       }
       yield return new WaitForSeconds(1);
       P1Cam.SetActive(false);
       cameraController.SetActive(true);
       MoveNamePlate.SetActive(false);
    }

    public IEnumerator PlayBuffAnimP2(Move move){
       cameraController.SetActive(false);
       P2Cam.SetActive(true);
       MoveNamePlate.GetComponentInChildren<Text>().text = move.name;
       MoveNamePlate.SetActive(true);
       if((player2MonsterInstance).GetComponent<Animator>() != null){
            (player2MonsterInstance).GetComponent<Animator>().SetBool("Attack 02", true);
       }
       yield return new WaitForSeconds(1);
       P2Cam.SetActive(false);
       cameraController.SetActive(true);

       MoveNamePlate.SetActive(false);
    }

    public IEnumerator PlayFaintAnimP2(){
       cameraController.SetActive(false);
       P2Cam.SetActive(true);
       if((player2MonsterInstance).GetComponent<Animator>() != null){
            (player2MonsterInstance).GetComponent<Animator>().SetBool("Die", true);
       }
       yield return new WaitForSeconds(1);
       Destroy(player2MonsterInstance);
    }

    public IEnumerator PlayAtkAnimP2(Move move){
       cameraController.SetActive(false);
       P2Cam.SetActive(true);
       MoveNamePlate.GetComponentInChildren<Text>().text = move.name;
       MoveNamePlate.SetActive(true);
       if((player2MonsterInstance).GetComponent<Animator>() != null){
            (player2MonsterInstance).GetComponent<Animator>().SetBool("Attack 01", true);
            StartCoroutine(AnimationStateRestore(player2MonsterInstance));
       }
       yield return new WaitForSeconds(2);
       P1Cam.SetActive(true);
       P2Cam.SetActive(false);
       if((player1MonsterInstance).GetComponent<Animator>() != null){
            (player1MonsterInstance).GetComponent<Animator>().SetBool("Take Damage", true);
            StartCoroutine(HPBarDepleteAnim(P1HPMeter, (float) player1Monster.currentHP / (float) player1Monster.maxHP));
            P1HPText.GetComponent<Text>().text = player1Monster.currentHP.ToString() + " / " + player1Monster.maxHP.ToString();
            StartCoroutine(AnimationStateRestore(player1MonsterInstance));
       }
       yield return new WaitForSeconds((player1MonsterInstance).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
       MoveNamePlate.SetActive(false);
       StartCoroutine(CameraStateRestore());
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

    IEnumerator HPBarFillAnim(GameObject img, float targetWidth){
        //P1HPMeter.GetComponent<Image>().rectTransform.localScale = new Vector3( (float) player1Monster.currentHP / (float) player1Monster.maxHP , 1, 1); 
        Debug.Log("Target Width: " + targetWidth);
        float aux = img.GetComponent<Image>().rectTransform.localScale.x;

        while(img.GetComponent<Image>().rectTransform.localScale.x < targetWidth){
            img.GetComponent<Image>().rectTransform.localScale = new Vector3(aux += 0.08f, 1, 1);
            yield return new WaitForSeconds(0.014f);
            if(img.GetComponent<Image>().rectTransform.localScale.x > 0.2f){
                img.GetComponent<Image>().color = new Color32(252, 232, 0, 255);
            }
            if(img.GetComponent<Image>().rectTransform.localScale.x > 0.5f){
                Debug.Log("Over Half");
                img.GetComponent<Image>().color = new Color32(42, 134, 46, 255);
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
            instance.GetComponent<Animator>().SetBool("Attack 01", false);
            instance.GetComponent<Animator>().SetBool("Take Damage", false);
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

