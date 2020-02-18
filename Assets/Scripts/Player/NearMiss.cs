#define DEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearMiss : MonoBehaviour
{
    public float bonusScoreAmount;
    public static Coroutine startNearMiss;
   
    private float time;

   
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("obstacle"))
            return;

        if (startNearMiss != null)
            StopCoroutine(startNearMiss);
        startNearMiss = StartCoroutine(  NMRoutine());
#if DEBUG
        TestDataSaver.Instance.numOfNearMiss++;
#endif
    }

    public IEnumerator NMRoutine()
    {
      
        yield return new WaitForSeconds(0.1f);
        if(!Score._instance.isPlaying)
            StartCoroutine(Score._instance.ScoreFlash(27));
        Score._instance.score += bonusScoreAmount;
    }
}