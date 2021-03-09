using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IControllable{
    void youveBeenTapped();
    void MoveTo(Ray ray, Touch touch,Vector3 destination);
    void RotateObject(float angle);
    void RotateObjectUpDownLeftRight();
    void ScaleObject();
    void Stop();
    void AccelerometerMove(Vector3 dir);
}
