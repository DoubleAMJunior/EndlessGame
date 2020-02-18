using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RounderMove : MonoBehaviour
{
    #region Inspector Atributes
    public List<Vector3> points;
    public List<float> speed;
    #endregion
    private Vector3 tempTransform;

    #region Setup Functions 
    private void OnEnable()
    {
        tempTransform = transform.localPosition;
        StartCoroutine(RoundMove());
    }
    private void OnDisable()
    {
        transform.localPosition = tempTransform;
    }
    #endregion
    IEnumerator RoundMove()
    {
        int i = 0;
        int j = 0;
        Vector3 point = points[0];
        point.z = transform.localPosition.z;
        while (true)
        {
            if (transform.localPosition == point)
            {
                point = points[i];
                point.z = transform.localPosition.z;
                i++;
                j++;
                if (i > points.Count - 1)
                {
                    i = 0;
                }

                if (j > speed.Count - 1)
                {
                    j = 0;
                }
            }

            
            transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                point, speed[j]);
            yield return new WaitForFixedUpdate();
        }
    }
}
