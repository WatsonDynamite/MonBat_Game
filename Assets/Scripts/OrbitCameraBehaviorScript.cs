using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCameraBehaviorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pivot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(pivot.transform.position, Vector3.up, 13 * Time.deltaTime);
    }
}
