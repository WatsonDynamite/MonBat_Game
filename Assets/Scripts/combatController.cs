using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatController : MonoBehaviour
{


    public GameObject player1Spawn;
    public GameObject player2Spawn;

    private Monster player1Monster;
    private Monster player2Monster;
    // Start is called before the first frame update
    void Start()
    {
        player1Monster = MonsterList.testMon1;
        player2Monster = MonsterList.testMon1;
        Instantiate((GameObject) player2Monster.model).transform.SetParent(player1Spawn.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

