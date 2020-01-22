
/*


THIS IS NOT USED ANYMORE BECAUSE ALL IT DID WAS CAUSE PROBLEMS
ALSO IT LOOKED LIKE SHIT


 */




using UnityEngine;
using System.Collections;

public class cameraShake : MonoBehaviour
{
    private float speedX = 1.2f;
    private float speedY = 1f;
    public float intensity = 0.5f;
    
    public Camera self;
    public Vector3 initPos;

    private void OnDisable() {
        
        
    }

    void Start(){
        initPos = transform.localPosition;
    }

    void OnEnable(){
        self.fieldOfView = 70;
        StartCoroutine(DramaticZoom());
    }

	void Update () {

        //transform.localPosition = new Vector3(initPos.x + Mathf.Sin(Time.time * speedX) * intensity, initialPosition.y + Mathf.Sin(Time.time * speedY) * intensity, 0f);
        transform.localPosition = new Vector3( initPos.x + Mathf.PerlinNoise(0, Time.time * speedX),
	                                           initPos.y + Mathf.PerlinNoise(1, Time.time * speedY),
	                                            0
                                            ) * intensity;
        
        
    }

    IEnumerator DramaticZoom(){
        yield return new WaitForSeconds(1.5f);
        while( self.fieldOfView > 60){
            self.fieldOfView -= 1f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1.2f);
        while( self.fieldOfView > 50){
            self.fieldOfView -= 1f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1.2f);
        while( self.fieldOfView > 35){
            self.fieldOfView -= 1f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }
}