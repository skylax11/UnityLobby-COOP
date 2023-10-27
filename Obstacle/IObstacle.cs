using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObstacle
{
    void GetRidOf(Vector3 direction, Transform transform);
}
