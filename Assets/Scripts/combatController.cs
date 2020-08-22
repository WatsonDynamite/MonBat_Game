﻿/* Probably the most complex script in the entire game, the CombatController, well... controls combat.
    However, it also controls the camera on the battlefield, the spots in which each monster will spawn,
    and basically every animation that plays.

    This code has a TON of problems and should be considered a barely functional prototype
    some issues include:
        - a whole bunch of code repetition
        - incompatibility with anything other than 1v1 fights
        - obviously, no multiplayer or netcode of any sort
        - a lot of hardcoded shit that really shouldn't be hardcoded

    Honestly, the fact that this works at all should have made me ellegible for some sort of prize.
    I dread having to comment this.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class combatController : MonoBehaviour {

        int turnCounter;  //counts the number of turns that have passed in battle. I think I only use this for the battle log, but it might come in handy.

        public GameObject player1Spawn; //this is the spawn point for player 1's monsters' models.
        public GameObject player2Spawn; //same but for player 2

        public List<Monster> player1Party; //list of Player 1's available monsters
        public List<Monster> player2Party; //same but for player 2

        public Monster player1Monster; //player 1's current active monster
        public Monster player2Monster; //you know the drill

        //these are the models that are loaded on the scene
        private GameObject player1MonsterInstance;
        private GameObject player2MonsterInstance;

        private List<Move> moveList; //list of moves for the current player

        // game objects for the name / health / stamina etc.
        [Header ("HP / STA")]
        public GameObject P1MonsterName;
        public GameObject P1HPText;
        public GameObject P1HPMeter;
        public GameObject MonstersLeftP1;

        public GameObject P2MonsterName;
        public GameObject P2HPText;
        public GameObject P2HPMeter;
        public GameObject MonstersLeftP2;

        // the Final Fantasy / Digimon style move name that shows up when a monster moves.
        public GameObject moveNamePlate;

        [Header ("Log Text")] // objects for the battle log that shows up in the corner.
        public GameObject logText;
        public GameObject scrollRect;

        [Header ("Attack Cams")] // the objects and controllers for the camera.
        public GameObject P1Cam;
        public GameObject P2Cam;
        public GameObject cameraController;

        [Header ("Hit / Secondary / Status effects")] // the objects for the animations / graphics that appear whenever a status or side effect happens. 
        public GameObject defDownIndP1;
        public GameObject defDownIndP2;
        public GameObject speedUpIndP1;
        public GameObject speedUpIndP2;
        public GameObject StatusEffectImageP1;
        public GameObject StatusEffectTurnCounterP1;
        
        public GameObject StatusEffectImageP2;
        public GameObject StatusEffectTurnCounterP2;

        [Header ("Monster Selection buttons")] // this is the list that shows up when the player wants to switch monsters.
        public GameObject monsterList;

        public bool isTurnInProgress;  //controls whether the animations for a turn are playing.
        public bool reloadUI = false;  //reloads the UI when set to true

        private List<IEnumerator> seq; //this is the sequence of animations and actions for a given turn. This needs to be constructed and then executed
        private float damageCalcConstant = 0.6f; //this is the constant for the damage formula.


        //these are the delegates for the different animations. Delegates are essentially variables that store functions. Very useful to avoid code repetitions.
        delegate IEnumerator AtkAnim (Move move, int dmg);
        delegate IEnumerator FaintAnim ();
        delegate IEnumerator BuffAnim (Move move);
        delegate IEnumerator DebuffAnim (Move move);
        delegate IEnumerator SwitchAnim ();

        void Awake () {

            //IT IS VERY IMPORTANT FOR THE PLAYER PARTIES IN-GAME TO NOT CONTAIN ANY MONSTER DIRECTLY FROM THE MONSTER LIST
            //THIS CAUSES A TON OF PROBLEMS WHEN THERE ARE DUPLICATE MONSTERS AS THE STAT CHANGES WILL WORK ON THE STATIC "TEMPLATE" AND NOT IN SEPARATE INSTANCES
            //ALWAYS INSTANTIATE NEW COPIES OF THE MONSTER FROM THE MONSTERLIST CLASS TO USE THEM IN GAMEPLAY!!!!!

            player1Party = new List<Monster> () { new Monster (MonsterList.testMon1), new Monster (MonsterList.testMon2), MonsterList.monsterNone, MonsterList.monsterNone, MonsterList.monsterNone, MonsterList.monsterNone };
            player2Party = new List<Monster> () { new Monster (MonsterList.testMon2), new Monster (MonsterList.testMon1), MonsterList.monsterNone, MonsterList.monsterNone, MonsterList.monsterNone, MonsterList.monsterNone };

            //sets the field monster to the first party member
            player1Monster = player1Party[0];
            player2Monster = player2Party[0];

            WriteToLog ("Player 1 sent out " + player1Monster.name + "!"); //obviously players will eventually have names
            WriteToLog ("Player 2 sent out " + player2Monster.name + "!");

        }

        // Start is called before the attacker frame update
        void Start () {
            turnCounter = 1;
            isTurnInProgress = false;
            //fill UI

            LoadHPPlates ();

            //spawn monsters
            player1MonsterInstance = Instantiate (player1Monster.model as GameObject, player1Spawn.transform.position, player1Spawn.transform.rotation);
            player1MonsterInstance.transform.SetParent (player1Spawn.transform);
            player2MonsterInstance = Instantiate (player2Monster.model as GameObject, player2Spawn.transform.position, player2Spawn.transform.rotation);
            player2MonsterInstance.transform.SetParent (player2Spawn.transform);
        }

        // Update is called once per frame
        void Update () { /* Since this controller is essentially a state machine, there's no need to do anything on a frame-by-frame basis. */}


        //here's where it gets REALLY messy:

        //MAIN COMBAT FUNCTIONS

        public IEnumerator ExecuteTurn (TurnAction player1Action, TurnAction player2Action) {
            Debug.Log ("Starting turn");
            if (isTurnInProgress) {
                yield break; //stops any attempts to execute this function when a turn is already in progress
            }
            seq = new List<IEnumerator> ();
            // determine turn order
            var speedDiff = player1Monster.SPEED.getTrueValue () - player2Monster.SPEED.getTrueValue ();
            //if positive, player 1 is attacker. If negative, player 2 is attacker. If 0, speed tie.

            WriteToLog ("--- TURN " + turnCounter + " ---");

            //currently, on a speed tie the turn order is picked at random. I will have to decide on something else later. I don't want it to be RNG.

            if (speedDiff == 0) {
                speedDiff = 1 * Mathf.Sign (Random.Range (-1, 1));
            }

            //here we check what are the actions each player performs this turn.
            //this is YandereDev levels of coding, but I haven't thought of anything better.

            if (player1Action.actionType == ActionType.MOVE && player2Action.actionType == ActionType.MOVE) {
                //both attack
                if (speedDiff > 0) {
                    //player 1 goes first
                    Debug.Log ("Both attack, speedDiff > 0");
                    seq.Add (DoMoves (player1Action.move, player1Monster, player2Monster));
                    seq.Add (DoMoves (player2Action.move, player2Monster, player1Monster));

                } else if (speedDiff < 0) {
                    //player 2 goes first
                    Debug.Log ("Both attack, speedDiff < 0");
                    seq.Add (DoMoves (player2Action.move, player2Monster, player1Monster));
                    seq.Add (DoMoves (player1Action.move, player1Monster, player2Monster));
                }
            } else if (player1Action.actionType == ActionType.SWITCH && player2Action.actionType == ActionType.MOVE) {
                //player1 switches, player2 attacks
                Debug.Log ("Player 1 switches, player 2 attacks");
                seq.Add (SwapMon (player1Monster, player1Action.monster));
                seq.Add (DoMoves (player2Action.move, player2Monster, player1Action.monster));

            } else if (player1Action.actionType == ActionType.MOVE && player2Action.actionType == ActionType.SWITCH) {
                //player2 switches, player1 attacks
                Debug.Log ("Player 2 switches, player 1 attacks");
                seq.Add (SwapMon (player2Monster, player2Action.monster));
                seq.Add (DoMoves (player1Action.move, player1Monster, player2Action.monster));

            } else if (player1Action.actionType == ActionType.SWITCH && player2Action.actionType == ActionType.SWITCH) {
                //both switch. Switching order is based on the speed of the current monster.

                if (speedDiff > 0) {
                    //player 1 goes first
                    Debug.Log ("Both switch, speedDiff > 0");
                    seq.Add (SwapMon (player1Monster, player1Action.monster));
                    seq.Add (SwapMon (player2Monster, player2Action.monster));

                } else if (speedDiff < 0) {
                    //player 2 goes first
                    Debug.Log ("Both switch, speedDiff < 0");
                    seq.Add (SwapMon (player2Monster, player2Action.monster));
                    seq.Add (SwapMon (player1Monster, player1Action.monster));
                }
            }

            //here we apply the effects of Status conditions, which happens in speed order.
            if (speedDiff > 0) {
                seq.Add (PerformStatusEffects (player1Monster));
                seq.Add (PerformStatusEffects (player2Monster));

            } else {
                seq.Add(PerformStatusEffects (player2Monster));
                seq.Add(PerformStatusEffects (player1Monster));
            }

            isTurnInProgress = true;
            foreach (var item in seq) {
                yield return item;
                if (player1Monster == null || player2Monster == null) { //checks if the monster that is about to act is dead, and stops that action from happening.
                    Debug.Log ("Someone died");
                    yield break;
                }
            }
            isTurnInProgress = false;
            turnCounter++;
            yield break;

        }

        public IEnumerator PerformStatusEffects (Monster monster) { //executes the effect of a status effect to a given monster
            if (monster.statusEffect != null) { 
                monster.statusEffect.turnCounter++; //increments turn counter for a status effect

                if (monster.statusEffect.turnCounter > monster.statusEffect.maxTurns) {
                    WriteToLog (monster.name + "'s status effect faded!");
                    monster.statusEffect = null;
                } else {
                    switch (monster.statusEffect.statusEffectType) { //checks which status effect is active
                        case StatusEffectEnum.POISONED:
                            WriteToLog (monster.name + " is hurt by poison!");
                            monster.receiveDamage ((int) ((monster.maxHP / 16f) * monster.statusEffect.turnCounter));
                                if (ReferenceEquals (monster, player1Monster)) {
                                    StartCoroutine (HPBarDepleteAnim (P1HPMeter, (float) player1Monster.currentHP / (float) player1Monster.maxHP));
                                        yield return PlayDebuffAnimP1 ();
                                    P1HPText.GetComponent<Text> ().text = player1Monster.currentHP.ToString () + " / " + player1Monster.maxHP.ToString ();
                                } else {
                                    StartCoroutine (HPBarDepleteAnim (P2HPMeter, (float) player2Monster.currentHP / (float) player2Monster.maxHP));
                                        yield return PlayDebuffAnimP2 ();
                                    P2HPText.GetComponent<Text> ().text = player2Monster.currentHP.ToString () + " / " + player2Monster.maxHP.ToString ();
                                }
                                yield return new WaitForSeconds(1);
                                break;
                            }
                    }
            }
            yield return new WaitForSeconds (0);
        }

            public IEnumerator SwapMon (Monster current, Monster newMon) { //this is for when you manually switch
                Debug.Log ("Doing Switching!");
                reloadUI = true;
                GameObject camr = P1Cam;
                SwitchAnim SwitchAnimDel = new SwitchAnim (PlayBuffAnimP1);
                Debug.Log (current.name);
                Debug.Log (newMon.name);
                float animationTime = 1f;


                //set up which monster we're switching
                if (ReferenceEquals (current, player1Monster)) {
                    //we're doing player 1
                    camr = P1Cam;
                    SwitchAnimDel = new SwitchAnim (PlayBuffAnimP1);   //right now there's no switching out animations so I use the Buff animation
                    WriteToLog ("Player 1 has swapped to " + newMon.name);
                } else {
                    //we're doing player 2
                    camr = P1Cam;
                    SwitchAnimDel = new SwitchAnim (PlayBuffAnimP2);  //right now there's no switching out animations so I use the Buff animation
                    WriteToLog ("Player 2 has swapped to " + newMon.name);
                }


                //execute what we've set up
                cameraController.SetActive (false); //stops the automatic camera swaps
                camr.SetActive (true); //changes the view to the static camera
                yield return SwitchAnimDel (); //play animation

                //this should be changed for the sake of no code repetition
                //also, player 2 can't switch yet, so that "else" block is horribly incomplete.
                if (ReferenceEquals (current, player1Monster)) {
                    player1Monster = null;
                    Destroy (player1MonsterInstance);
                    player1Monster = newMon;
                    player1MonsterInstance = Instantiate (player1Monster.model as GameObject, player1Spawn.transform.position, player1Spawn.transform.rotation);
                } else {
                    player2Monster = null;
                    player2Monster = newMon;
                }

               
                yield return SwitchAnimDel ();  //plays the sending out animation (right now it's the same as the switch out animation)
                LoadHPPlates (); //reloads the HP plates
                yield return new WaitForSeconds (animationTime);

                //resets cameras
                camr.SetActive (false);
                reloadUI = false;
                cameraController.SetActive (true);
            }

            public IEnumerator SummonNewMon (Monster newMon, int player) //handles sending out a new monster after current mon faints
            {
                Debug.Log ("Doing Summoning!");
                reloadUI = true;
                GameObject camr = P1Cam;
                SwitchAnim SwitchAnimDel = new SwitchAnim (PlayBuffAnimP1);
                float animationTime = 1f;

                switch (player) {
                    case 1:
                        //we're doing player 1
                        camr = P1Cam;
                        SwitchAnimDel = new SwitchAnim (PlayBuffAnimP1);
                        WriteToLog ("Player 1 has sent out " + newMon.name);
                        break;
                    case 2:
                        //we're doing player 2
                        camr = P2Cam;
                        SwitchAnimDel = new SwitchAnim (PlayBuffAnimP2);
                        WriteToLog ("Player 2 has sent out " + newMon.name);
                        break;
                }

                cameraController.SetActive (false);
                camr.SetActive (true);

                switch (player) {
                    case 1:
                        player1Monster = newMon;
                        player1MonsterInstance = Instantiate (player1Monster.model as GameObject, player1Spawn.transform.position, player1Spawn.transform.rotation);
                        break;

                    case 2:
                        player2Monster = newMon;
                        player2MonsterInstance = Instantiate (player2Monster.model as GameObject, player2Spawn.transform.position, player2Spawn.transform.rotation);
                        break;
                }

                yield return SwitchAnimDel ();
                LoadHPPlates ();
                yield return new WaitForSeconds (animationTime);

                camr.SetActive (false);
                reloadUI = false;
                cameraController.SetActive (true);
                isTurnInProgress = false;
            }

            public IEnumerator DoMoves (Move move, Monster attacker, Monster defender) { //execute turn if both monsters are attacking
                if (attacker != null) {
                    Debug.Log ("Doing Moves");
                    WriteToLog (attacker.name + " used " + move.name + "!");

                    var isSTAB = false;
                    var dmg = 0;
                    var effectiveness = (TypeUtils.Effectiveness (move.type, defender.type1) * TypeUtils.Effectiveness (move.type, defender.type2));
                    Debug.Log ("Effectiveness: " + effectiveness);
                    AtkAnim AtkAnimDel1st;
                    FaintAnim FaintAnimDel1st;
                    BuffAnim BuffAnimDel1st;

                    AtkAnim AtkAnimDel2nd;
                    FaintAnim FaintAnimDel2nd;
                    BuffAnim BuffAnimDel2nd;

                    if (move.cat != Category.STATUS) {
                        switch (effectiveness) {
                            case 4f:
                                WriteToLog ("Ultra effective!");
                                break;
                            case 2f:
                                WriteToLog ("Super effective!");
                                break;
                            case 0.5f:
                                WriteToLog ("Not very effective...");
                                break;
                            case 0.25f:
                                WriteToLog ("Ultra ineffective...");
                                break;
                            case 0f:
                                WriteToLog ("No effect!");
                                break;
                        }
                    }

                    Debug.Log ("Who's attacking? " + ReferenceEquals (attacker, player1Monster));
                    if (ReferenceEquals (attacker, player1Monster)) {
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

                            var dmg1 = (int) Mathf.Floor ((float) ((move.power) * (isSTAB ? 1.5f : 1f)) * (attacker.ATK.getTrueValue () / defender.DEF.getTrueValue ()) * damageCalcConstant * effectiveness);

                            defender.receiveDamage (dmg1);
                            yield return AtkAnimDel1st (move, dmg1);
                            break;

                        case Category.SPECIAL:
                            var dmg2 = (int) Mathf.Floor ((float) ((move.power) * (isSTAB ? 1.5f : 1f)) * (attacker.spATK.getTrueValue () / defender.spDEF.getTrueValue ()) * damageCalcConstant * (TypeUtils.Effectiveness (move.type, defender.type1) * TypeUtils.Effectiveness (move.type, defender.type2)));
                            Debug.Log ("First attacks defender: " + dmg2);
                            defender.receiveDamage (dmg2);
                            //Debug.Log(move.secondaryEffects[0]);
                            yield return AtkAnimDel1st (move, dmg2);
                            break;

                        case Category.STATUS:
                            //nothing special. All it does is the buff / debuff animations which already happen with other moves

                            yield return BuffAnimDel1st (move);

                            break;
                    }

                    if (defender.currentHP <= 0) {
                        yield return FaintAnimDel2nd ();
                        //turn is over
                        yield break;
                    }

                    if (move.secondaryEffects != null) {
                        foreach (SecondaryEffect se in move.secondaryEffects) {
                            if (se != null) {
                                if (se.type == SecondaryEffectType.OTHER) {
                                    yield return ApplyEffect (se, defender);
                                }else {
                                    yield return ApplyEffect (se, attacker);
                                }

                            }
                        }
                    }

                }
            }

            public IEnumerator ApplyEffect (SecondaryEffect effect, Monster monster) { //apply side effects of moves
                Debug.Log ("Effect: " + effect);
                switch (effect.type) {
                    case SecondaryEffectType.SELF:
                        switch (effect.effect) {
                            case SecondaryEffectEffect.HEALING_HALF:
                                WriteToLog (monster.name + " healed for " + ((int) monster.maxHP / 2f).ToString () + " HP!");
                                monster.healDamage ((int) (monster.maxHP / 2f));
                                if (ReferenceEquals (monster, player1Monster)) {
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
                                if (ReferenceEquals (monster, player1Monster)) {
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
                                Debug.Log ("Defense down test: " + ReferenceEquals (monster, player1Monster));
                                if (ReferenceEquals (monster, player1Monster)) {
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
                            case SecondaryEffectEffect.POISON_FIVE:
                                Debug.Log ("trying to Poison enemy for 5 turns!");
                                Debug.Log(ReferenceEquals (monster, player1Monster));
                                if (ReferenceEquals (monster, player1Monster)) {
                                    Debug.Log("Player1Monster.StatusEffect: " + player1Monster.statusEffect);
                                    if(player1Monster.statusEffect == null){
                                        WriteToLog(player1Monster.name + " has been poisoned!");
                                        player1Monster.statusEffect = new StatusEffect(StatusEffectEnum.POISONED, 5);
                                    }else{
                                        WriteToLog("But it failed!");
                                    }
                                }else{
                                    if(player2Monster.statusEffect == null){
                                        WriteToLog(player2Monster.name + " has been poisoned!");
                                        player2Monster.statusEffect = new StatusEffect(StatusEffectEnum.POISONED, 5);
                                    }else{
                                        WriteToLog("But it failed!");
                                    }
                                }
                            break;

                        }
                        break;
                }
                yield return new WaitForSeconds (0);
            }

            public IEnumerator ShowNewMonsterSelector () //shows the UI for when the player loses a monster and needs to select a new one
            {
                isTurnInProgress = true;

                monsterList.SetActive (true);

                yield return new WaitForSeconds (0);

            }

            public IEnumerator ChangeNewPlayer2Monster () {
                List<Monster> auxList = new List<Monster> ();
                //obtain every non-dead monster from the party
                foreach (Monster mon in player2Party) {
                    if (!(mon.Compare (MonsterList.monsterNone)) && mon.currentHP != 0) {
                        auxList.Add (mon);
                    }
                }
                System.Random ran = new System.Random ();
                Monster monAux = auxList[ran.Next (auxList.Count)];

                yield return SummonNewMon (monAux, 2);

            }

            //WRITE TO LOG

            public void WriteToLog (string str) {
                logText.GetComponentInChildren<Text> ().text += (str + "\n");
                scrollRect.GetComponentInChildren<ScrollRect> ().normalizedPosition = new Vector2 (0, 0);
            }

            //REFRESH UI ELEMENTS

            public void LoadHPPlates () {

                LoadMonsterScoreBoards ();

                P1HPText.GetComponent<Text> ().text = player1Monster.currentHP.ToString () + " / " + player1Monster.maxHP.ToString ();
                StartCoroutine (HPBarInstantSetAnim (P1HPMeter, (float) player1Monster.currentHP / (float) player1Monster.maxHP));
                P2HPText.GetComponent<Text> ().text = player2Monster.currentHP.ToString () + " / " + player2Monster.maxHP.ToString ();
                StartCoroutine (HPBarInstantSetAnim (P2HPMeter, (float) player2Monster.currentHP / (float) player2Monster.maxHP));
                P1MonsterName.GetComponent<Text> ().text = player1Monster.name;
                P2MonsterName.GetComponent<Text> ().text = player2Monster.name;
            }

            public void LoadMonsterScoreBoards () {
                MonstersLeftP1.GetComponent<Text> ().text = GetRemainingMonstersParty (1).ToString ();
                MonstersLeftP2.GetComponent<Text> ().text = GetRemainingMonstersParty (2).ToString ();
            }

            public int GetRemainingMonstersParty (int player) {
                int val = 0;
                switch (player) {
                    case 1:
                        foreach (Monster mon in player1Party) {
                            if (mon != null) {
                                if (mon.currentHP != 0) {
                                    val++;
                                }
                            }
                        }

                        break;
                    case 2:
                        foreach (Monster mon in player2Party) {
                            if (mon != null) {
                                if (mon.currentHP != 0) {
                                    val++;
                                }
                            }
                        }
                        break;
                }
                return val;
            }

            //ATK ANIMS

            public IEnumerator PlayAtkAnimP1 (Move move, int dmg) {
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
                    WriteToLog ("\t" + dmg.ToString () + " damage!");
                    P2HPText.GetComponent<Text> ().text = player2Monster.currentHP.ToString () + " / " + player2Monster.maxHP.ToString ();
                    yield return new WaitForSeconds (0.4f);
                    StartCoroutine (AnimationStateRestore (player2MonsterInstance));
                }
                yield return new WaitForSeconds (0.7f);
                moveNamePlate.SetActive (false);
                StartCoroutine (CameraStateRestore ());
            }

            public IEnumerator PlayAtkAnimP2 (Move move, int dmg) {
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
                    WriteToLog ("\t" + dmg + " damage!");
                    P1HPText.GetComponent<Text> ().text = player1Monster.currentHP.ToString () + " / " + player1Monster.maxHP.ToString ();
                    yield return new WaitForSeconds (0.4f);
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
                //present monster GUI
                LoadMonsterScoreBoards ();
                yield return ShowNewMonsterSelector ();

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
                LoadMonsterScoreBoards ();
                yield return ChangeNewPlayer2Monster ();

            }

            //ANIMS FOR CASTING STATUS MOVES

            public IEnumerator PlayStatusAnimP1 (Move move) {
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

            IEnumerator HPBarInstantSetAnim (GameObject img, float targetWidth) {
                img.GetComponent<Image> ().rectTransform.localScale = new Vector3 (targetWidth, 1, 1);
                if (img.GetComponent<Image> ().rectTransform.localScale.x > 0.5f) {
                    img.GetComponent<Image> ().color = new Color32 (42, 134, 46, 255);
                } else if (img.GetComponent<Image> ().rectTransform.localScale.x > 0.2f) {
                    img.GetComponent<Image> ().color = new Color32 (252, 232, 0, 255);
                } else {
                    img.GetComponent<Image> ().color = new Color32 (255, 0, 0, 255);
                }

                yield return new WaitForSeconds (1);
            }

            IEnumerator HPBarFillAnim (GameObject img, float targetWidth) {
                float aux = img.GetComponent<Image> ().rectTransform.localScale.x;
                while (img.GetComponent<Image> ().rectTransform.localScale.x < targetWidth) {
                    img.GetComponent<Image> ().rectTransform.localScale = new Vector3 (aux += 0.08f, 1, 1);
                    yield return new WaitForSeconds (0.014f);
                    if (img.GetComponent<Image> ().rectTransform.localScale.x > 0.2f) {
                        img.GetComponent<Image> ().color = new Color32 (252, 232, 0, 255);
                    }
                    if (img.GetComponent<Image> ().rectTransform.localScale.x > 0.5f) {
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
                if (instance != null) {
                    instance.GetComponent<Animator> ().SetBool ("Attack 01", false);
                    instance.GetComponent<Animator> ().SetBool ("Take Damage", false);
                }
            }

            public List<Move> getP1Moves () {
                if (player1Monster != null) {
                    return new List<Move> { player1Monster.move1, player1Monster.move2, player1Monster.move3, player1Monster.move4 };
                }
                return null;
            }

            public List<Move> getP2Moves () {
                if (player1Monster != null) {
                    return new List<Move> { player2Monster.move1, player2Monster.move2, player2Monster.move3, player2Monster.move4 };
                }
                return null;
            }

            public List<Monster> getP1Party () {
                return player1Party;
            }

            public List<Monster> getP2Party () {
                return player2Party;
            }
        }