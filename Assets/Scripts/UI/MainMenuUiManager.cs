using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUiManager : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject leaderBoards;
    public GameObject shopMenu;

    public void OnLeaderboardsPressed()
    {
        leaderBoards.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnSettingsPressed()
    {
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Play()
    {
        StartCoroutine(SceneManagement.Instance.LoadScene(2, 1));
    }

    public void OnShopPressed()
    {
        shopMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
