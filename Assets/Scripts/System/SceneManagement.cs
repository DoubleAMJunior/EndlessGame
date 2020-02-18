using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance;
    public static int CurrentOverlayScene = 1;
    
    private void Awake()
    {
        Instance = this;

        StartCoroutine(LoadScene(CurrentOverlayScene, -1));
    }
    
    

    public IEnumerator LoadScene(int sceneToload,int sceneToUnLoad)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneToload,LoadSceneMode.Additive);
        while (!op.isDone)
        {
            //Can be used to do things mid loading the next part
            yield return null;
        }

        if (sceneToUnLoad!=-1)
        {
            SceneManager.UnloadSceneAsync(sceneToUnLoad);   
            yield return null;
        }
        //after the unload of the next scene
    }
}
