using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInfiniteGenerator : MonoBehaviour
{
    public int PreviousScene;
    public int NextScene;
    
    private void OnTriggerEnter(Collider other)
    {
    //    StartCoroutine(SceneManagement.Instance.LoadScene(NextScene,PreviousScene));
    }
}
