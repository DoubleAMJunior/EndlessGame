using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    #region InspectorAtributes
    public float animationSpeed; 
    public List<GameObject> phone;
    public List<GameObject> touchPoints;
    public List<GameObject> touchPointDest;
    #endregion


    private void Start()
    {
        StartCoroutine(TutorialSEquence());
    }

    private IEnumerator TutorialSEquence()
    {
        //flash the points in phone
        foreach (GameObject point in touchPoints)
        {
            point.GetComponent<Animator>().SetTrigger("Flash");
        }
        yield return new WaitForSeconds(1);
        //fade the phone and move the points to the new position
        foreach (var ph in phone)
        {
            ph.GetComponent<Animator>().SetTrigger("Fade");    
        }

        bool reached = false;
        while (!reached)
        {
            for (int i=0;i<touchPoints.Count;i++)
            {
                touchPoints[i].transform.position=Vector3.MoveTowards(touchPoints[i].transform.position
                ,touchPointDest[i].transform.position,animationSpeed);
            }

            if (Vector3.Distance( touchPoints[0].transform.position ,touchPointDest[0].transform.position) <= animationSpeed)
            {
                reached = true;
            }

            yield return null;
        }
        yield return new WaitForSeconds(1);
        foreach (GameObject point in touchPoints)
        {
            point.GetComponent<Animator>().SetTrigger("Flash");
        }
        yield return new WaitForSeconds(1);
        foreach (GameObject point in touchPoints)
        {
            point.GetComponent<Animator>().SetTrigger("Fade");
        }
    }
}
