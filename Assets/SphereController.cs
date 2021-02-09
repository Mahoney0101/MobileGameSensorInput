using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour, IControllable
{
    Vector3 drag_position;
    // Start is called before the first frame update
    void Start()
    {
        drag_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, drag_position, 1f);
    }
    public void youveBeenTapped()
    {
        transform.position += Vector3.right;
    }
     public void MoveTo(Ray ray, Touch touch,Vector3 destination){
     drag_position = destination;
 }
     public void Stop(){
        transform.Translate(Vector3.zero, 0);
     }
}
