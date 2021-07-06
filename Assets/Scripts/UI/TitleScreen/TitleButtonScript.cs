using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonScript : MonoBehaviour
{
    public void Exitgame() {  
        Debug.Log("exitgame");  
        Application.Quit();  
    }

    public void PlayMatch() {
        Debug.Log("playing game");
        SceneManager.LoadScene("battleScene");
    }  
}
