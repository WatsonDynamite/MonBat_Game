using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffIndicatorScript : MonoBehaviour
{   
    public int behaviorType; //0 for scrolling down (usually for debuffs), 1 for scrolling up (usually for buffs);
    public Vector3 OGPosition;
    private float travel_dist = 0.6f;
    private float speed = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        OGPosition = transform.position;
    }

    
    void OnEnable()
    {
        Debug.Log("Enabling Indicator");
        OGPosition = transform.position;
        Debug.Log("OGPosition: " + OGPosition);
        switch(behaviorType){
            case 0: StartCoroutine(FloatDown());
                break;
            case 1: StartCoroutine(FloatUp());
                break;
        }
        
    }

    void Update(){
        /*
        Debug.Log("Current Position: " + transform.position.y);
        if(transform.position.y >= OGPosition.y - travel_dist){
            
           
        }else{
           
            
        }
        */
        //transform.LookAt(Camera.);
    }

    void OnDisable()
    {
        transform.position = OGPosition;
    }

    IEnumerator FloatDown(){
        yield return new WaitForSeconds(0.4f);
        Debug.Log("Moving indicator");
        while(transform.position.y > OGPosition.y - travel_dist){
         transform.position = Vector3.MoveTowards(transform.position, new Vector3(OGPosition.x , OGPosition.y - travel_dist, OGPosition.z),  speed * Time.deltaTime);
         Debug.Log("Transform.position: " + transform.position);
         yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("Removing Indicator");
        gameObject.SetActive(false);
    }

    IEnumerator FloatUp(){
        yield return new WaitForSeconds(0.4f);
        Debug.Log("Moving indicator");
        while(transform.position.y < OGPosition.y + travel_dist){
         transform.position = Vector3.MoveTowards(transform.position, new Vector3(OGPosition.x , OGPosition.y + travel_dist, OGPosition.z),  speed * Time.deltaTime);
         yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.4f);
         Debug.Log("Removing Indicator");
        gameObject.SetActive(false);
    }


}
