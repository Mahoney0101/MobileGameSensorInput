using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleController : MonoBehaviour, IControllable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
}
