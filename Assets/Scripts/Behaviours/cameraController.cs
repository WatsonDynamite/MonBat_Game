//this script creates a list of all the cameras. 
//So long as this script is enabled, the viewport will switch between every camera on the list.

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

    private List<KeyValuePair<GameObject, float>> cameras;
    
    void OnEnable()
    {
       cameras = new List<KeyValuePair<GameObject, float>>{ new KeyValuePair<GameObject, float>(overShoulder, 10), 
                                                            new KeyValuePair<GameObject, float>(orbit, 7), 
                                                            new KeyValuePair<GameObject, float>(faceZoom1Camera, 8),
                                                            new KeyValuePair<GameObject, float>(splitScCamera, 5),
                                                            new KeyValuePair<GameObject, float>(faceZoom2Camera, 8),
                                                            new KeyValuePair<GameObject, float>(orbitMon1, 7),
                                                            new KeyValuePair<GameObject, float>(orbitMon2, 7)};

       //the key is the camera object, the value is the number of seconds the camera remains active before it switches.

       StartCoroutine(CameraStart());
    }

    void Update(){
        
    }


    //this is a cute shuffle function I sniffed out on StackOverflow. It shuffles the list of cameras.
    public static List<KeyValuePair<GameObject, float>> Fisher_Yates_CardDeck_Shuffle (List<KeyValuePair<GameObject, float>>aList) {
 
         System.Random _random = new System.Random ();
 
         KeyValuePair<GameObject, float> myGO;
 
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

    IEnumerator CameraStart(){
        overShoulder.SetActive(true);
        yield return new WaitForSeconds(15.0f);
        overShoulder.SetActive(false);
        StartCoroutine(CameraSwap());
        
        yield return null;
    }

    IEnumerator CameraSwap(){
        List<KeyValuePair<GameObject, float>> cameraShuffled = Fisher_Yates_CardDeck_Shuffle(cameras);
        foreach(KeyValuePair<GameObject, float> cam in cameraShuffled){
            cam.Key.SetActive(true);
            yield return new WaitForSeconds(cam.Value);
            cam.Key.SetActive(false);
        }
        StartCoroutine(CameraSwap());
        yield return null;
    }

}
