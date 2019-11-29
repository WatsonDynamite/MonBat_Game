using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject overShoulder;
    public GameObject orbit;
    public GameObject faceZoom1Camera;
    public GameObject faceZoom2Camera;
    public GameObject splitScCamera;
    public GameObject orbitMon1;
    public GameObject orbitMon2;

    private List<GameObject> cameras;
    // Start is called before the first frame update
    void Start()
    {
       cameras = new List<GameObject>{overShoulder, orbit, faceZoom1Camera,  splitScCamera, faceZoom2Camera, orbitMon1, orbitMon2};
       StartCoroutine(CameraSwap());
    }

    void Update(){
        
    }

         public static List<GameObject> Fisher_Yates_CardDeck_Shuffle (List<GameObject>aList) {
 
         System.Random _random = new System.Random ();
 
         GameObject myGO;
 
         int n = aList.Count;
         for (int i = 0; i < n; i++)
         {
             // NextDouble returns a random number between 0 and 1.
             // ... It is equivalent to Math.random() in Java.
             int r = i + (int)(_random.NextDouble() * (n - i));
             myGO = aList[r];
             aList[r] = aList[i];
             aList[i] = myGO;
         }
 
         return aList;
     }


    IEnumerator CameraSwap(){
        List<GameObject> cameraShuffled = Fisher_Yates_CardDeck_Shuffle(cameras);
        foreach(GameObject cam in cameraShuffled){
            Debug.Log(cam);
            cam.SetActive(true);
            yield return new WaitForSeconds(6.0f);
            cam.SetActive(false);
        }
        StartCoroutine(CameraSwap());
        yield return null;
    }

}
