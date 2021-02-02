using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManagerScript : MonoBehaviour
{
    IControllable selectedObject;
    private float tapBegan;
    private bool tapMoved;
    private float tapTime = 0.2f;  
    float starting_distance_to_selected_object;  
    Ray new_position;
    void Start()
    {
        // GameObject ourCameraPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        // ourCameraPlane.transform.up = (Camera.main.transform.position - ourCameraPlane.transform.position).normalized;
    }

    void Update()
    {
    if (Input.touchCount > 0) 
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
                        object_hit.youveBeenTapped();
                        Debug.Log("YOUVE BEEN TAPPED");
                        selectedObject = object_hit;
                        starting_distance_to_selected_object = Vector3.Distance(Camera.main.transform.position, info.transform.position);
                    }
                    else{
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
                selectedObject.MoveTo(Input.touches[0],new_position.GetPoint(starting_distance_to_selected_object));
                break;

            case TouchPhase.Stationary:
                // new_position = Camera.main.ScreenPointToRay(Input.touches[0].position);
                // selectedObject.MoveTo(Input.touches[0],new_position.GetPoint(starting_distance_to_selected_object));
                break;
            case TouchPhase.Ended:
                // selectedObject.Stop();
                break;
        }
        }
    }
    }
}
