using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffSpriteController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
      
        StartCoroutine(WaitAndDisable());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private IEnumerator WaitAndDisable(){
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
