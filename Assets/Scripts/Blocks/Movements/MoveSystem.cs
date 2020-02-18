using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    
    private static MoveSystem _instance;

    public static MoveSystem instance
    {
        get
        {
            if (!_instance) _instance = FindObjectOfType<MoveSystem>();
            return _instance;
        }
    }
    
    #region balance
    [SerializeField]private  float startSpeed=1;
    public  float delta=0.004f;
    #endregion   

    #region privateProperties

    [SerializeField]private float moveSpeed ;    
    private List<Transform> movingObjects=new List<Transform>();
    #endregion
    
    #region interface
    public void subscribe(List<Transform> add)
    {
        movingObjects.AddRange(add);
    }

    public void subscribe(Transform add)
    {
        
        movingObjects.Add(add);
    }

    public void unSubscribe(Transform sub)
    {
        
        movingObjects.Remove(sub);
        
    }

    public void changeSpeed(float speed)
    {
        StartCoroutine(speedRoutine(speed));
    }
    
    #endregion
    
    #region privateFunctions
    
    private IEnumerator speedRoutine(float speed)
    {
        while (speed-moveSpeed>0)
        {
            moveSpeed += delta;
            yield return new WaitForFixedUpdate();
        }
    }
    private void Awake()
    {
        if (!_instance) _instance = this;
    }

    private void Start()
    {
        StartCoroutine(speedRoutine(startSpeed));
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < movingObjects.Count; i++)
        {
            if (movingObjects[i]!=null)
            {
               movingObjects[i].position -=  Vector3.forward * moveSpeed;
            }
            else
            {
                movingObjects.RemoveAt(i);
            }
        }
    }
    
    #endregion
    
}
