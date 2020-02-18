using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelPoolHelper : MonoBehaviour
{
    public static float delayTime = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            StartCoroutine(delayPool());
        }
    }

    IEnumerator delayPool()
    {
        yield return new WaitForSeconds(delayTime);
        TunnelManager.instance.CreateNew(transform.parent.gameObject);
    }
}
