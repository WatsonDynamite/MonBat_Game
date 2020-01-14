using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffIndicatorScript : MonoBehaviour
{
    public Vector3 OGPosition;
    public int travel_dist;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        OGPosition = transform.position;
    }

    
    void OnEnable()
    {
        OGPosition = transform.position;
    }

    void Update(){
        if(transform.position.y > OGPosition.y - travel_dist){
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(OGPosition.x , OGPosition.y - travel_dist, OGPosition.z),  speed * Time.deltaTime);
        }else{
            gameObject.SetActive(false);
        }

    }

    void OnDisable()
    {
        transform.position = OGPosition;
    }


}
