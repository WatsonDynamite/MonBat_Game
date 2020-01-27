using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour {

    int turnCounter;

    public GameObject player1Spawn;
    public GameObject player2Spawn;

    public Monster player1Monster;
    public Monster player2Monster;

    //this is the model that is loaded on the scene
    private GameObject player1MonsterInstance;
    private GameObject player2MonsterInstance;

    private List<Move> moveList;

    //name / health / stamina etc.
    [Header ("HP / STA")]
    public GameObject P1MonsterName;
    public GameObject P2MonsterName;
    public GameObject P1HPText;
    public GameObject P1HPMeter;
    public GameObject P2HPText;
    public GameObject P2HPMeter;
    public GameObject moveNamePlate;

    [Header ("Log Text")]
    public GameObject logText;
    public GameObject scrollRect;

    [Header ("Attack Cams")]
    public GameObject P1Cam;
    public GameObject P2Cam;
    public GameObject cameraController;

    [Header ("Hit / Secondary / Status effects")]
    public GameObject defDownIndP1;
    public GameObject defDownIndP2;
    public GameObject speedUpIndP1;
    public GameObject speedUpIndP2;

    public bool isTurnInProgress;

    public enum Actions {
        ATTACK,
        SWITCH,

        FAINT
    }

    delegate IEnumerator AtkAnim (Move move);
    delegate IEnumerator FaintAnim ();
    delegate IEnumerator BuffAnim (Move move);
    delegate IEnumerator DebuffAnim (Move move);

    void Awake () {
        player1Monster = MonsterList.testMon1;
        player2Monster = MonsterList.testMon2;

        WriteToLog ("Player 1 sent out " + player1Monster.name + "!");
        WriteToLog ("Player 2 sent out " + player2Monster.name + "!");

    }

    // Start is called before the attacker frame update
    void Start () {
        turnCounter = 1;
        isTurnInProgress = false;
        //fill UI
        P1HPText.GetComponent<Text> ().text = player1Monster.currentHP.ToString () + " / " + player1Monster.maxHP.ToString ();
        P2HPText.GetComponent<Text> ().text = player2Monster.currentHP.ToString () + " / " + player2Monster.maxHP.ToString ();
        P1MonsterName.GetComponent<Text> ().text = player1Monster.name;
        P2MonsterName.GetComponent<Text> ().text = player2Monster.name;

        //spawn monsters
        player1MonsterInstance = Instantiate (player1Monster.model as GameObject, player1Spawn.transform.position, player1Spawn.transform.rotation);
        player1MonsterInstance.transform.SetParent (player1Spawn.transform);
        player2MonsterInstance = Instantiate (player2Monster.model as GameObject, player2Spawn.transform.position, player2Spawn.transform.rotation);
        player2MonsterInstance.transform.SetParent (player2Spawn.transform);
    }

    // Update is called once per frame
    void Update () { }

    public IEnumerator ExecuteTurn (Move player1Move, Move player2Move) {
        if (isTurnInProgress) {
            yield break;
        }
        List<IEnumerator> seq = new List<IEnumerator> ();
        // determine turn order
        var speedDiff = player1Monster.SPEED.getTrueValue () - player2Monster.SPEED.getTrueValue ();
        //if positive, player 1 is attacker. If negative, player 2 is attacker. If 0, speed tie.

        //currently, nothing happens on a speed tie. I will have to decide on that later. I don't want it to be RNG.

        WriteToLog ("--- TURN " + turnCounter + " ---");
        if (speedDiff > 0) {
            Debug.Log ("Peep");
            seq.Add (DoMoves (player1Move, player1Monster, player2Monster));
            seq.Add (DoMoves (player2Move, player2Monster, player1Monster));

        } else if (speedDiff < 0) {
            Debug.Log ("Poop");
            seq.Add (DoMoves (player2Move, player2Monster, player1Monster));
            seq.Add (DoMoves (player1Move, player1Monster, player2Monster));
        }

        foreach (var item in seq) {
            yield return item;
            if(player1Monster.HP.value == 0 || player2Monster.HP.value == 0){
                Debug.Log("SHIET");
                yield break;
            }
        }
        turnCounter++;
        yield break;

    }

    public IEnumerator DoMoves (Move move, Monster attacker, Monster defender) {
        if(attacker != null){
        Debug.Log ("Doing Moves");
        WriteToLog (attacker.name + " used " + move.name + "!");
        isTurnInProgress = true;
        var isSTAB = false;
        var dmg = 0;
        AtkAnim AtkAnimDel1st;
        FaintAnim FaintAnimDel1st;
        BuffAnim BuffAnimDel1st;

        AtkAnim AtkAnimDel2nd;
        FaintAnim FaintAnimDel2nd;
        BuffAnim BuffAnimDel2nd;

        if (attacker == player1Monster) {
            AtkAnimDel1st = new AtkAnim (PlayAtkAnimP1);
            FaintAnimDel1st = new FaintAnim (PlayFaintAnimP1);
            BuffAnimDel1st = new BuffAnim (PlayStatusAnimP1);

            AtkAnimDel2nd = new AtkAnim (PlayAtkAnimP2);
            FaintAnimDel2nd = new FaintAnim (PlayFaintAnimP2);
            BuffAnimDel2nd = new BuffAnim (PlayStatusAnimP2);

        } else {

            AtkAnimDel1st = new AtkAnim (PlayAtkAnimP2);
            FaintAnimDel1st = new FaintAnim (PlayFaintAnimP2);
            BuffAnimDel1st = new BuffAnim (PlayStatusAnimP2);

            AtkAnimDel2nd = new AtkAnim (PlayAtkAnimP1);
            FaintAnimDel2nd = new FaintAnim (PlayFaintAnimP1);
            BuffAnimDel2nd = new BuffAnim (PlayStatusAnimP1);
        }

        if (attacker.type1 == move.type || attacker.type2 == move.type) {
            //is stab
            isSTAB = true;
        } else {
            isSTAB = false;
        }

        //FIRST ATTACKS SECOND
        switch (move.cat) {
            case Category.PHYSICAL:
                var dmg1 = (int) Mathf.Floor ((float) ((move.power) * (isSTAB ? 1.5f : 1f)) * (attacker.ATK.getTrueValue () / defender.DEF.getTrueValue ()) * 0.3f * (TypeUtils.Effectiveness (move.type, defender.type1) * TypeUtils.Effectiveness (move.type, defender.type2)));
                
                defender.receiveDamage (dmg1);
                //Debug.Log(move.secondaryEffects[0]);
                yield return AtkAnimDel1st (move);
                /* 
                foreach (SecondaryEffect se in move.secondaryEffects) {
                    if (se != null) {
                        var affectsOthers = false;
                        if (se.type == SecondaryEffectType.OTHER) {
                            affectsOthers = true;
                        }

                        if (affectsOthers) {
                            yield return BuffAnimDel1st (move);
                            yield return ApplyEffect (se, defender);
                        } else {
                            yield return BuffAnimDel1st (move);
                            yield return ApplyEffect (se, attacker);
                        }
                    }
                }
                */
                break;

            case Category.SPECIAL:
                var dmg2 = (int) Mathf.Floor ((float) ((move.power) * (isSTAB ? 1.5f : 1f)) * (attacker.spATK.getTrueValue () / defender.spDEF.getTrueValue ()) * 0.3f * (TypeUtils.Effectiveness (move.type, defender.type1) * TypeUtils.Effectiveness (move.type, defender.type2)));
                Debug.Log ("First attacks defender: " + dmg2);
                defender.receiveDamage (dmg2);
                //Debug.Log(move.secondaryEffects[0]);
                yield return AtkAnimDel1st (move);
                /* 
                foreach (SecondaryEffect se in move.secondaryEffects) {
                    if (se != null) {
                        var affectsOthers = false;
                        if (se.type == SecondaryEffectType.OTHER) {
                            affectsOthers = true;
                        }

                        if (affectsOthers) {
                            yield return BuffAnimDel1st (move);
                            yield return ApplyEffect (se, defender);
                        } else {
                            yield return BuffAnimDel1st (move);
                            yield return ApplyEffect (se, attacker);
                        }
                    }
                }
                */
                break;

            case Category.STATUS:
                //nothing special. All it does is the buff / debuff animations which already happen with other moves
                
                            yield return BuffAnimDel1st (move);
                /* 
                foreach (SecondaryEffect se in move.secondaryEffects) {
                    if (se != null) {
                        var affectsOthers = false;
                        if (se.type == SecondaryEffectType.OTHER) {
                            affectsOthers = true;
                        }

                        if (affectsOthers) {
                            yield return BuffAnimDel1st (move);
                            yield return ApplyEffect (se, defender);
                        } else {
                            yield return BuffAnimDel1st (move);
                            yield return ApplyEffect (se, attacker);
                        }
                    }
                } */
                break;
        }

        if (defender.currentHP <= 0) {
            yield return FaintAnimDel2nd ();
            //turn is over
            yield break;
        }

        if(move.secondaryEffects != null){
        foreach (SecondaryEffect se in move.secondaryEffects) {
                    if (se != null) {
                        var affectsOthers = false;
                        if (se.type == SecondaryEffectType.OTHER) {
                            affectsOthers = true;
                        }

                        if (affectsOthers) {
                            yield return ApplyEffect (se, defender);
                        } else {
                            yield return ApplyEffect (se, attacker);
                        }
                    }
        }
        }


        /* 
        if (defender.type1 == secondMove.type || defender.type2 == secondMove.type) {
            //is stab
            isSTAB = true;
        } else {
            isSTAB = false;
        }

        
        //SECOND ATTACKS FIRST
        switch (secondMove.cat) {
            case Category.PHYSICAL:
                var dmg3 = (int) Mathf.Floor (((secondMove.power) * (isSTAB ? 1.5f : 1f)) * (defender.ATK.getTrueValue() / attacker.DEF.getTrueValue()) * 0.3f * (TypeUtils.Effectiveness (secondMove.type, attacker.type1) * TypeUtils.Effectiveness (secondMove.type, attacker.type2)));
                attacker.receiveDamage (dmg3);
                Debug.Log ("Second attacks attacker: " + dmg3);
                yield return AtkAnimDel2nd (secondMove);
                break;

            case Category.SPECIAL:
                attacker.receiveDamage ((int) Mathf.Floor (((secondMove.power) * (isSTAB ? 1.5f : 1f)) * (defender.spATK.getTrueValue() / attacker.spDEF.getTrueValue()) * 0.3f * (TypeUtils.Effectiveness (secondMove.type, attacker.type1) * TypeUtils.Effectiveness (secondMove.type, attacker.type2))));
                yield return AtkAnimDel2nd (secondMove);
                break;

            case Category.STATUS:
                foreach (SecondaryEffect se in secondMove.secondaryEffects) {
                var affectsOthers = false;
                if (se.type == SecondaryEffectType.OTHER) {
                    affectsOthers = true;
                }
                if (affectsOthers) {
                        yield return BuffAnimDel2nd (secondMove);
                        yield return ApplyEffect (se, attacker);
                } else {
                        yield return BuffAnimDel2nd (secondMove);
                        yield return ApplyEffect (se, defender);
                 }
                }

                break;

        }

        if (attacker.currentHP <= 0) {
            yield return FaintAnimDel1st ();

            yield break; //turn is over
        }
        */
        isTurnInProgress = false;

        }
    }

    public IEnumerator ApplyEffect (SecondaryEffect effect, Monster monster) {
        Debug.Log ("Effect: " + effect);
        switch (effect.type) {
            case SecondaryEffectType.SELF:
                switch (effect.effect) {
                    case SecondaryEffectEffect.HEALING_HALF:
                        WriteToLog (monster.name + " healed for " + (int) (monster.maxHP / 2f) + " HP!");
                        monster.healDamage ((int) (monster.maxHP / 2f));
                        if (monster == player1Monster) {
                            StartCoroutine (HPBarFillAnim (P1HPMeter, (float) player1Monster.currentHP / (float) player1Monster.maxHP));
                            P1HPText.GetComponent<Text> ().text = player1Monster.currentHP.ToString () + " / " + player1Monster.maxHP.ToString ();
                        } else {
                            StartCoroutine (HPBarFillAnim (P2HPMeter, (float) player2Monster.currentHP / (float) player2Monster.maxHP));
                            P2HPText.GetComponent<Text> ().text = player2Monster.currentHP.ToString () + " / " + player2Monster.maxHP.ToString ();
                        }
                        break;

                    case SecondaryEffectEffect.BOOST_SPEED_1:
                        Debug.Log ("SPEED UP!");
                        WriteToLog (monster.name + "'s SPEED increased!");
                        if (monster == player1Monster) {
                            //playdebuffanimp1
                            speedUpIndP1.SetActive (true);
                            player1Monster.SPEED.buffStat ();
                            yield return PlayBuffAnimP1 ();
                        } else {
                            //playdebuffanimp2
                            speedUpIndP2.SetActive (true);
                            player2Monster.SPEED.buffStat ();
                            yield return PlayBuffAnimP2 ();
                        }
                        break;

                }
                break;

            case SecondaryEffectType.OTHER:
                Debug.Log ("effect.effect: " + effect.effect);
                switch (effect.effect) {
                    case SecondaryEffectEffect.LOWER_DEF_1:
                        Debug.Log ("DEFENSE DOWN!");
                        WriteToLog (monster.name + "'s DEFENSE dropped!");
                        if (monster == player1Monster) {
                            //playdebuffanimp1
                            defDownIndP1.SetActive (true);
                            player1Monster.DEF.debuffStat ();
                            yield return PlayDebuffAnimP1 ();
                        } else {
                            //playdebuffanimp2
                            defDownIndP2.SetActive (true);
                            player2Monster.DEF.debuffStat ();
                            yield return PlayDebuffAnimP2 ();
                        }
                        break;

                }
                break;
        }
        yield return new WaitForSeconds (0);
    }

    //WRITE TO LOG

    public void WriteToLog (string str) {
        logText.GetComponentInChildren<Text> ().text += (str + "\n");
        scrollRect.GetComponentInChildren<ScrollRect> ().normalizedPosition = new Vector2 (0, 0);
    }

    //ATK ANIMS

    public IEnumerator PlayAtkAnimP1 (Move move) {
        cameraController.SetActive (false);
        P1Cam.SetActive (true);
        moveNamePlate.GetComponentInChildren<Text> ().text = move.name;
        moveNamePlate.SetActive (true);
        if ((player1MonsterInstance).GetComponent<Animator> () != null) {
            (player1MonsterInstance).GetComponent<Animator> ().SetBool ("Attack 01", true);
            StartCoroutine (AnimationStateRestore (player1MonsterInstance));
        }
        yield return new WaitForSeconds (2);
        P2Cam.SetActive (true);
        P1Cam.SetActive (false);
        if ((player2MonsterInstance).GetComponent<Animator> () != null) {
            (player2MonsterInstance).GetComponent<Animator> ().SetBool ("Take Damage", true);
            StartCoroutine (HPBarDepleteAnim (P2HPMeter, (float) player2Monster.currentHP / (float) player2Monster.maxHP));
            WriteToLog ("\t" + (player2Monster.maxHP - player2Monster.currentHP) + " damage!");
            P2HPText.GetComponent<Text> ().text = player2Monster.currentHP.ToString () + " / " + player2Monster.maxHP.ToString ();
            yield return new WaitForSeconds(0.4f);
            StartCoroutine (AnimationStateRestore (player2MonsterInstance));
        }
        yield return new WaitForSeconds (0.7f);
        moveNamePlate.SetActive (false);
        StartCoroutine (CameraStateRestore ());
    }

    public IEnumerator PlayAtkAnimP2 (Move move) {
        cameraController.SetActive (false);
        P2Cam.SetActive (true);
        moveNamePlate.GetComponentInChildren<Text> ().text = move.name;
        moveNamePlate.SetActive (true);
        if ((player2MonsterInstance).GetComponent<Animator> () != null) {
            (player2MonsterInstance).GetComponent<Animator> ().SetBool ("Attack 01", true);
            StartCoroutine (AnimationStateRestore (player2MonsterInstance));
        }
        yield return new WaitForSeconds (2);
        P1Cam.SetActive (true);
        P2Cam.SetActive (false);
        if ((player1MonsterInstance).GetComponent<Animator> () != null) {
            (player1MonsterInstance).GetComponent<Animator> ().SetBool ("Take Damage", true);
            StartCoroutine (HPBarDepleteAnim (P1HPMeter, (float) player1Monster.currentHP / (float) player1Monster.maxHP));
            WriteToLog ("\t" + (player1Monster.maxHP - player1Monster.currentHP) + " damage!");
            P1HPText.GetComponent<Text> ().text = player1Monster.currentHP.ToString () + " / " + player1Monster.maxHP.ToString ();
            yield return new WaitForSeconds(0.4f);
            StartCoroutine (AnimationStateRestore (player1MonsterInstance));
        }
        yield return new WaitForSeconds ((player1MonsterInstance).GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).length);
        moveNamePlate.SetActive (false);
        StartCoroutine (CameraStateRestore ());
    }

    //FAINT ANIMS

    public IEnumerator PlayFaintAnimP1 () {
        cameraController.SetActive (false);
        P1Cam.SetActive (true);
        if ((player1MonsterInstance).GetComponent<Animator> () != null) {
            (player1MonsterInstance).GetComponent<Animator> ().SetBool ("Die", true);
        }
        yield return new WaitForSeconds (1);
        Destroy (player1MonsterInstance);
        player1Monster = null;
    }

    public IEnumerator PlayFaintAnimP2 () {
        cameraController.SetActive (false);
        P2Cam.SetActive (true);
        if ((player2MonsterInstance).GetComponent<Animator> () != null) {
            (player2MonsterInstance).GetComponent<Animator> ().SetBool ("Die", true);
        }
        yield return new WaitForSeconds (1);
        Destroy (player2MonsterInstance);
        player2Monster = null;
    }

    //ANIMS FOR CASTING STATUS MOVES

    public IEnumerator PlayStatusAnimP1 (Move move) {
        Debug.Log ("PlayStatusAnimP1");
        cameraController.SetActive (false);
        P1Cam.SetActive (true);
        moveNamePlate.GetComponentInChildren<Text> ().text = move.name;
        moveNamePlate.SetActive (true);
        if ((player1MonsterInstance).GetComponent<Animator> () != null) {
            (player1MonsterInstance).GetComponent<Animator> ().SetBool ("Attack 02", true);
        }
        yield return new WaitForSeconds (1.3f);
        P1Cam.SetActive (false);
        cameraController.SetActive (true);
        moveNamePlate.SetActive (false);
    }

    public IEnumerator PlayStatusAnimP2 (Move move) {
        Debug.Log ("PlayStatusAnimP2");
        cameraController.SetActive (false);
        P2Cam.SetActive (true);
        moveNamePlate.GetComponentInChildren<Text> ().text = move.name;
        moveNamePlate.SetActive (true);
        if ((player2MonsterInstance).GetComponent<Animator> () != null) {
            (player2MonsterInstance).GetComponent<Animator> ().SetBool ("Attack 02", true);
        }
        yield return new WaitForSeconds (1.4f);
        P2Cam.SetActive (false);
        cameraController.SetActive (true);

        moveNamePlate.SetActive (false);
    }

    //ANIMS FOR RECEIVING DEBUFFS

    public IEnumerator PlayDebuffAnimP1 () {
        Debug.Log ("playDebuffAnimP1");
        cameraController.SetActive (false);
        P1Cam.SetActive (true);
        if ((player1MonsterInstance).GetComponent<Animator> () != null) {
            (player1MonsterInstance).GetComponent<Animator> ().SetBool ("Take Damage", true);
        }
        yield return new WaitForSeconds (1.3f);
        P1Cam.SetActive (false);
        cameraController.SetActive (true);
    }

    public IEnumerator PlayDebuffAnimP2 () {
        Debug.Log ("playDebuffAnimP2");
        cameraController.SetActive (false);
        P2Cam.SetActive (true);
        if ((player2MonsterInstance).GetComponent<Animator> () != null) {
            (player2MonsterInstance).GetComponent<Animator> ().SetBool ("Take Damage", true);
        }
        yield return new WaitForSeconds (1.3f);
        P2Cam.SetActive (false);
        cameraController.SetActive (true);
    }

    //ANIMS FOR RECEIVING BUFFS

    public IEnumerator PlayBuffAnimP1 () {
        Debug.Log ("playDebuffAnimP1");
        cameraController.SetActive (false);
        P1Cam.SetActive (true);
        if ((player1MonsterInstance).GetComponent<Animator> () != null) {
            (player1MonsterInstance).GetComponent<Animator> ().SetBool ("Attack 02", true);
        }
        yield return new WaitForSeconds (1.3f);
        P1Cam.SetActive (false);
        cameraController.SetActive (true);
    }

    public IEnumerator PlayBuffAnimP2 () {
        Debug.Log ("playDebuffAnimP2");
        cameraController.SetActive (false);
        P2Cam.SetActive (true);
        if ((player2MonsterInstance).GetComponent<Animator> () != null) {
            (player2MonsterInstance).GetComponent<Animator> ().SetBool ("Attack 02", true);
        }
        yield return new WaitForSeconds (1.3f);
        P2Cam.SetActive (false);
        cameraController.SetActive (true);
    }


    IEnumerator HPBarDepleteAnim (GameObject img, float targetWidth) {
        yield return new WaitForSeconds (0.1f);
        Debug.Log ("Target Width: " + targetWidth);
        float aux = img.GetComponent<Image> ().rectTransform.localScale.x;

        while (img.GetComponent<Image> ().rectTransform.localScale.x > targetWidth) {
            img.GetComponent<Image> ().rectTransform.localScale = new Vector3 (aux -= 0.08f, 1, 1);
            yield return new WaitForSeconds (0.014f);
            if (img.GetComponent<Image> ().rectTransform.localScale.x < 0.5f) {
                img.GetComponent<Image> ().color = new Color32 (252, 232, 0, 255);
            }
            if (img.GetComponent<Image> ().rectTransform.localScale.x < 0.2f) {
                img.GetComponent<Image> ().color = new Color32 (255, 0, 0, 255);

            }
        }

        yield return new WaitForSeconds (1);
    }

    IEnumerator HPBarFillAnim (GameObject img, float targetWidth) {
        Debug.Log ("Target Width: " + targetWidth);
        float aux = img.GetComponent<Image> ().rectTransform.localScale.x;
        while (img.GetComponent<Image> ().rectTransform.localScale.x < targetWidth) {
            img.GetComponent<Image> ().rectTransform.localScale = new Vector3 (aux += 0.08f, 1, 1);
            yield return new WaitForSeconds (0.014f);
            if (img.GetComponent<Image> ().rectTransform.localScale.x > 0.2f) {
                img.GetComponent<Image> ().color = new Color32 (252, 232, 0, 255);
            }
            if (img.GetComponent<Image> ().rectTransform.localScale.x > 0.5f) {
                Debug.Log ("Over Half");
                img.GetComponent<Image> ().color = new Color32 (42, 134, 46, 255);
            }
        }
    }

    /* 
        public static IEnumerator ExecuteTurnAnimations(params IEnumerator[] sequence){
                for (int i = 0; i < sequence.Length; ++i){
                     while(sequence[i].MoveNext())
                            yield return sequence[i].Current;
                }
        }
    */

    public IEnumerator CameraStateRestore () {
        P1Cam.SetActive (false);
        P2Cam.SetActive (false);
        cameraController.SetActive (true);
        yield return new WaitForSeconds (0);
    }

    //pause attack animation (testing)

    IEnumerator AnimationStateRestore (GameObject instance) {
        yield return new WaitForSeconds (3.5f);
        if(instance != null){
        instance.GetComponent<Animator> ().SetBool ("Attack 01", false);
        instance.GetComponent<Animator> ().SetBool ("Take Damage", false);
        }
    }

    public List<Move> getP1Moves () {
        if (player1Monster != null) {
            return new List<Move> {player1Monster.move1, player1Monster.move2, player1Monster.move3, player1Monster.move4};
        }
        return null;
    }

    public List<Move> getP2Moves () {
        if (player1Monster != null) {
            return new List<Move> { player2Monster.move1, player2Monster.move2, player2Monster.move3, player2Monster.move4 };
        }
        return null;
    }

}