using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotatorMove : MonoBehaviour
{
    #region Inspector Atributes
    public int direction;
    public float speed;
    [Space]
    public bool useOrginalTransform =true;
    public Vector3 pointToRotateAround;
    public Vector3 axis = Vector3.forward;
    public float timeToRotateInOneDirection;
    #endregion

    #region Private Atributes
    private float angle = 360.0f; // Degree per time unit
    private float time = 1.0f;
    private float timer;
    #endregion
    
    void Update()
    {
        if (useOrginalTransform)
        {
            pointToRotateAround = transform.position;
        }
        timer += Time.deltaTime;
        if (timer >= timeToRotateInOneDirection && timeToRotateInOneDirection != 0) 
        {
            speed *= -1;
            timer = 0;
        }
        transform.RotateAround(pointToRotateAround, axis, direction*speed * angle * Time.deltaTime / time);
    }
}
