using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour, IControllable
{
void Start()
{
}

 void Update () 
 {

 }

 public void youveBeenTapped()
 {
    transform.position += Vector3.down;
 }
 public void MoveTo(Ray ray, Touch touch, Vector3 destination){
    // get the touch position from the screen touch to world point
    Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
    // lerp and set the position of the current object to that of the touch, but smoothly over time.
    transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime*10);
 }
 public void Stop(){
    transform.Translate(Vector3.zero, 0);
 }
}
