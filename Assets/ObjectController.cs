using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour, IControllable
{
Vector3 initialScale;
float initialDistance=1f;

void Start()
{

}  
 void Update () 
 {

 }
    public void RotateObject(float angle)
{
   float turnAngleDelta = angle;
	Quaternion desiredRotation = transform.rotation;

	if (Mathf.Abs(turnAngleDelta) > 0) { 
		Vector3 rotationDeg = Vector3.zero;
		rotationDeg.z = -turnAngleDelta;
		desiredRotation *= Quaternion.Euler(-rotationDeg);
	}
	transform.rotation = desiredRotation;
}

 public void youveBeenTapped()
 {
    transform.position += Vector3.down;
 }
 public void MoveTo(Ray ray, Touch touch, Vector3 destination){
    Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
    transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime*10);
 }

 public void ScaleObject()
 {
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
      transform.localScale = (initialScale * factor);
   }
 }
 public void Stop(){
    transform.Translate(Vector3.zero, 0);
 }
}
