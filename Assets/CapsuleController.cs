using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleController : MonoBehaviour, IControllable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    Vector3 initialScale;
    float initialDistance=1f;

    // Update is called once per frame
    void Update()
    {
        
    }
    public void youveBeenTapped(){
    }
    public void MoveTo(Ray ray, Touch touch, Vector3 destination){  
    this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    RaycastHit hit;
    if(Physics.Raycast(ray, out hit)){
    destination = hit.point;
    transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime*10);
        }
    }
    public void Stop(){
    this.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void RotateObject(float turnAngleDelta)
{
	//float pinchAmount = 0;
	Quaternion desiredRotation = transform.rotation;
 
	// if (Mathf.Abs(pinchDistanceDelta) > 0) { // zoom
	// 	pinchAmount = pinchDistanceDelta;
	// }
 
	if (Mathf.Abs(turnAngleDelta) > 0) { // rotate
		Vector3 rotationDeg = Vector3.zero;
		rotationDeg.z = -turnAngleDelta;
		desiredRotation *= Quaternion.Euler(-rotationDeg);
	}
	// not so sure those will work:
	transform.rotation = desiredRotation;
}
public void ScaleObject()
 {
        float min = 0.6f;
        if(transform.localScale.x <= min){
                transform.localScale = new Vector3(0.7f,0.7f,0.7f);
                return;
            }
     	var touchZero = Input.GetTouch(0); 
        var touchOne = Input.GetTouch(1);

        if(touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled  
           || touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled) 
        {
            return;
        }

        if(touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
        {
            initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
            initialScale = transform.localScale;
        }
        else
        {
            var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
            if(Mathf.Approximately(initialDistance, 0)) return;

            var factor = currentDistance / initialDistance;
            transform.localScale = initialScale * factor;
        }
 }
}
