#define DEBUG
using System.Collections;
using UnityEngine;

public class MovingEnviromentEnd : MonoBehaviour
{
    public static float delayTime = 1;

    private GameObject movingEnviromentGameObject;
    
    private void Start()
    {
        movingEnviromentGameObject = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (movingEnviromentGameObject.CompareTag("TunelEnd"))
            {
                StartCoroutine(delayPool());
            }
            else 
            {
#if DEBUG
             TestDataSaver.Instance.add(transform.parent.name);   
#endif
                if (ObstacleFieldGenerator.instance != null)
                {
                    ObstacleFieldGenerator.instance.CreateNew();
                    ObstacleFieldGenerator.instance.ReturnToPool(movingEnviromentGameObject);
                }
            }
        }
    }

    private WaitForSeconds waiter= new WaitForSeconds(delayTime);
    IEnumerator delayPool()
    {
        yield return waiter;
        TunnelManager.instance.CreateNew(movingEnviromentGameObject);
    }
}

