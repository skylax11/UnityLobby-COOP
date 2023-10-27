using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour , IObstacle
{
    public void GetRidOf(Vector3 direction, Transform transform) => 
        transform.position = Vector3.Lerp(transform.position, transform.position - direction, Time.deltaTime * 6f);

}
