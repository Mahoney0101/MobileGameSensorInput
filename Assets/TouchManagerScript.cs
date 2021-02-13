using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManagerScript : MonoBehaviour
{
    Vector2 lastTouchPosition;
    IControllable selectedObject;
    private float tapBegan;
    private bool tapMoved;
    private float tapTime = 0.2f;  
    float starting_distance_to_selected_object;  
    Ray new_position;
    float TouchZoomSpeed = 0.1f;
    float ZoomMinBound = 0.1f;
    float ZoomMaxBound = 179.9f;
    Camera cam;
    const float pinchTurnRatio = Mathf.PI / 2;
    const float minTurnAngle = 0;
    const float pinchRatio = 1;

    static public float turnAngle;
    private Vector3 initialScale;
    private float initialDistance;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
    if (Input.touchCount == 2 && selectedObject == null)
    {
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);
                // get touch position from the previous frame
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance (tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance (tZero.position, tOne.position);

                // get offset value
                float deltaDistance = oldTouchDistance - currentTouchDistance;
                Zoom (deltaDistance, TouchZoomSpeed);
                if(cam.fieldOfView < ZoomMinBound) 
                    {
                        cam.fieldOfView = 0.1f;
                    }
                    else
                    if(cam.fieldOfView > ZoomMaxBound ) 
                    {
                        cam.fieldOfView = 179.9f;
                    }
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
                        Debug.Log("YOUVE BEEN TAPPED");
                        selectedObject = object_hit;
                        Renderer renderer = info.transform.GetComponent<Renderer>();
                        renderer.material.SetColor("_Color",Color.red);
                        starting_distance_to_selected_object = Vector3.Distance(Camera.main.transform.position, info.transform.position);
                    }
                    else{
                        Renderer renderer = info.transform.GetComponent<Renderer>();
                        renderer.material.SetColor("_Color",Color.white);
                        selectedObject = null;
                        Debug.Log("Deselected object");
                    }
                }
            }
        }
        //assume selected object is not null. object selected. drag code here
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
    if(Input.touchCount == 2 && selectedObject != null)
     {
        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);
        selectedObject.RotateObject(Calculate());
        selectedObject.ScaleObject();
     }
    
}

	const float minPinchDistance = 0;
 
	const float panRatio = 1;
	const float minPanDistance = 0;
 
	static public float pinchDistanceDelta;

	static public float pinchDistance;

    /*
    Calculate Method
    Author: Caue Rego (cawas) 
    The code has been modified from original
    */
	static public float Calculate () {
        float turnAngleDelta;
		pinchDistance = pinchDistanceDelta = 0;
		turnAngle = turnAngleDelta = 0;
 
		if (Input.touchCount == 2) {
			Touch touch1 = Input.touches[0];
			Touch touch2 = Input.touches[1];
			if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved) {
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
        return turnAngleDelta;
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
    
    void Zoom(float deltaMagnitudeDiff, float speed)
    {
        cam.fieldOfView += deltaMagnitudeDiff * speed;
        // set min and max value of Clamp function upon your requirement
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, ZoomMinBound, ZoomMaxBound);
    }
}
