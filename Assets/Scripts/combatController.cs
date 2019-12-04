using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatController : MonoBehaviour
{


    public GameObject player1Spawn;
    public GameObject player2Spawn;

    private Monster player1Monster;
    private Monster player2Monster;

    private GameObject player1MonsterInstance;
    private GameObject player2MonsterInstance;
    // Start is called before the first frame update
    void Start()
    {
        player1Monster = MonsterList.testMon1;
        player2Monster = MonsterList.testMon2;
        player1MonsterInstance = Instantiate(player1Monster.model as GameObject, player1Spawn.transform.position, player1Spawn.transform.rotation);
        player1MonsterInstance.transform.SetParent(player1Spawn.transform);
        player2MonsterInstance = Instantiate(player2Monster.model as GameObject, player2Spawn.transform.position, player2Spawn.transform.rotation);
        player2MonsterInstance.transform.SetParent(player2Spawn.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayAnim(){
       if((player1MonsterInstance).GetComponent<Animator>() != null){
            (player1MonsterInstance).GetComponent<Animator>().SetBool("attacking", true);
            StartCoroutine(AnimationStateRestore());
       }
    }

    IEnumerator AnimationStateRestore(){
            yield return new WaitForSeconds(1.5f);
            (player1MonsterInstance).GetComponent<Animator>().SetBool("attacking", false);
    }
}

