using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour, IControllable
{
    Vector3 drag_position;
    Vector3 initialScale;
    float initialDistance=1f;
    void Start()
    {
    drag_position = transform.position;
    }

    void Update()
    {
    transform.position = Vector3.Lerp(transform.position, drag_position, 1f);
    }
    public void youveBeenTapped()
    {
    transform.position += Vector3.right;
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
    public void MoveTo(Ray ray, Touch touch,Vector3 destination)
    {
     drag_position = destination;
    }
    public void Stop()
    {
        transform.Translate(Vector3.zero, 0);
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
    public void AccelerometerMove(Vector3 dir)
    {
    float speed = 15f;
    transform.Translate(dir * speed);
    }

    public void RotateObjectUpDownLeftRight()
    {
    Touch touch = Input.touches[0];
    Touch touchOne = Input.touches[1];

    if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled  
    || touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled) 
    {
        return;
    }
    if(touch.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
    {

    }
    else
    {
        transform.eulerAngles += new Vector3((touch.deltaPosition.y+touchOne.deltaPosition.y)/2, (touch.deltaPosition.x+touchOne.deltaPosition.x)/2, 0);
    }
    }
}
