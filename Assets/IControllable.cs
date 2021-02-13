using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IControllable{
    void youveBeenTapped();
    void MoveTo(Ray ray, Touch touch,Vector3 destination);
    void RotateObject(float turnAngleDelta);
    void ScaleObject();
    void Stop();
}
