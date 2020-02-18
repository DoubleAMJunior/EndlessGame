using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfTutorial : MonoBehaviour
{
    public int currentScene;
    private void OnTriggerEnter(Collider other)
    {
        SceneManagement.CurrentOverlayScene = currentScene;
        PlayerProfile.Instance.LevelProgress = currentScene;
    }
}
