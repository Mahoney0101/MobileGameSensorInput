using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManagerScript : MonoBehaviour
{
    const float pinchRatio = 1;
	const float minPinchDistance = 0;
    IControllable selectedObject;
    private float tapBegan;
    private bool tapMoved;
    private float tapTime = 0.2f;  
    float starting_distance_to_selected_object;  
    Ray new_position;
    public Camera cam;
    const float pinchTurnRatio = Mathf.PI / 2;
    const float minTurnAngle = 0;
    static public float turnAngle;
    Vector3 originalCamPosition;
    static public float pinchDistanceDelta;

	static public float pinchDistance;
    float initialDistance = 1f;

    Vector3 initialPosition;
    Quaternion originalCamRotation;
    float speed = 10.0f;
    void Start()
    {
        cam = Camera.main;
        originalCamPosition = cam.transform.position;
        originalCamRotation = cam.transform.rotation;

    }
    

    void Update()
    {
    if(Input.touchCount == 0 && selectedObject != null)
    {
        Vector3 dir = Vector3.zero;
        dir.x = -Input.acceleration.y;
        dir.z = Input.acceleration.x;
        if (dir.sqrMagnitude > 1){
            dir.Normalize();
        }
        dir *= Time.deltaTime;
        

        selectedObject.AccelerometerMove(dir);
    }
    if(Input.touchCount == 1 && selectedObject == null)
        {
            Touch touch = Input.GetTouch(0);
            cam.transform.Translate(touch.deltaPosition * -0.1f); 
        }
    if (Input.touchCount == 1) 
    {
        Touch touch = Input.touches[0];
        RaycastHit info;
        Ray ourRay = Camera.main.ScreenPointToRay(touch.position);
        Debug.DrawRay(ourRay.origin, 30 * ourRay.direction);
        if (touch.phase == TouchPhase.Began)
        {
            tapBegan = Time.time;
            tapMoved = false;
        }

        if (touch.phase == TouchPhase.Moved)
        {
            tapMoved = true;

        }
        if (touch.phase == TouchPhase.Ended && tapMoved == false)
        {
            float tapLength = Time.time - tapBegan;
            if (tapLength <= tapTime && Physics.Raycast(ourRay, out info))
            {
                IControllable object_hit = info.transform.GetComponent<IControllable>();
                if(object_hit != null)
                {
                    if(selectedObject == null){
                        selectedObject = object_hit;
                        Renderer renderer = info.transform.GetComponent<Renderer>();
                        renderer.material.SetColor("_Color",Color.red);
                        starting_distance_to_selected_object = Vector3.Distance(Camera.main.transform.position, info.transform.position);
                    }
                    else{
                        Renderer renderer = info.transform.GetComponent<Renderer>();
                        renderer.material.SetColor("_Color",Color.white);
                        selectedObject = null;
                    }
                }
            }
        }
        if(selectedObject != null){
        switch(Input.touches[0].phase)
        {
            case TouchPhase.Began:
                break;
            case TouchPhase.Moved:
                new_position = Camera.main.ScreenPointToRay(Input.touches[0].position);
                selectedObject.MoveTo(ourRay, Input.touches[0], new_position.GetPoint(starting_distance_to_selected_object));
                break;
            case TouchPhase.Stationary:
                // new_position = Camera.main.ScreenPointToRay(Input.touches[0].position);
                // selectedObject.MoveTo(Input.touches[0],new_position.GetPoint(starting_distance_to_selected_object));
                break;
            case TouchPhase.Ended:
                selectedObject.Stop();
                break;
        }
        }
    }
    if (Input.touchCount == 2 && selectedObject == null)
    {
                RotateCamera(Calculate());
                MoveCam();
    }

    if(Input.touchCount == 2 && selectedObject != null)
     {
        selectedObject.RotateObject(Calculate());
        selectedObject.ScaleObject();
     }
    
}
    /*
    Calculate Method
    Author: Caue Rego (cawas) 
    The code has been modified from original
    */
	static public float Calculate () {
        float turnAngleDelta;
		turnAngle = turnAngleDelta = 0;
 
		if (Input.touchCount == 2) {
			Touch touch1 = Input.touches[0];
			Touch touch2 = Input.touches[1];
			if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved) {
                float prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition,
				                                      touch2.position - touch2.deltaPosition);
                pinchDistanceDelta = pinchDistance - prevDistance;
 
				// ... if it's greater than a minimum threshold, it's a pinch!
				if (Mathf.Abs(pinchDistanceDelta) > minPinchDistance) {
					pinchDistanceDelta *= pinchRatio;
				} else {
					pinchDistance = pinchDistanceDelta = 0;
				}
				turnAngle = Angle(touch1.position, touch2.position);
				float prevTurn = Angle(touch1.position - touch1.deltaPosition,
				                       touch2.position - touch2.deltaPosition);
				turnAngleDelta = Mathf.DeltaAngle(prevTurn, turnAngle);
 
				// ... if it's greater than a minimum threshold, it's a turn!
				if (Mathf.Abs(turnAngleDelta) > minTurnAngle) {
					turnAngleDelta *= pinchTurnRatio;
				} else {
					turnAngle = turnAngleDelta = 0;
				}
			}
		}
        return turnAngleDelta
;
	}

    static private float Angle (Vector2 pos1, Vector2 pos2) {
		Vector2 from = pos2 - pos1;
		Vector2 to = new Vector2(1, 0);
 
		float result = Vector2.Angle( from, to );
		Vector3 cross = Vector3.Cross( from, to );
 
		if (cross.z > 0) {
			result = 360f - result;
		}
 
		return result;
	}


    void RotateCamera(float turnAngleDelta){
        Quaternion desiredRotation = cam.transform.rotation;

        if (Mathf.Abs(turnAngleDelta) > 0) { 
            Vector3 rotationDeg = Vector3.zero;
            rotationDeg.z = -turnAngleDelta;
            desiredRotation *= Quaternion.Euler(-rotationDeg);
        }
	    cam.transform.rotation = desiredRotation;
    }
    void PanCameraUpDown(){

    }

    void PanCameraLeftRight(){

    }


    public void ResetCameraPosition(){
        cam.transform.position = originalCamPosition;
        cam.transform.rotation = originalCamRotation;
    }

    public void MoveCam()
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
            initialPosition = cam.transform.position;
        }
        else
        {
            var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
            if(Mathf.Approximately(initialDistance,currentDistance)) return;
            if(Mathf.Approximately(initialDistance-currentDistance, 0)) return;

            cam.transform.position += Vector3.forward * (initialDistance-currentDistance) * .01f;
        }
    }
}
