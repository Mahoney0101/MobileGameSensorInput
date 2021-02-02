using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IControllable{
    void youveBeenTapped();
    void MoveTo(Touch touch,Vector3 destination);
    void Stop();
}
