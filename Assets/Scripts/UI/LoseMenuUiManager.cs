using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoseMenuUiManager : MonoBehaviour
{
    public static LoseMenuUiManager Instance
    {
        get { return _instance ?? (_instance = FindObjectOfType<LoseMenuUiManager>()); }
    }

    #region InspectorAtributes
    public Text score;
    public Text highscore;
    public bool newHighscore;
    public GameObject LoseMenu;
    #endregion
    
    private static LoseMenuUiManager _instance;
   
    
    public void BackToMenu()
    {
    
        SceneManagement.CurrentOverlayScene = 1;
        SceneManager.LoadScene("basePlayerScene");
    }

    public void Restart()
    {
  
        SceneManagement.CurrentOverlayScene = 2;
        SceneManager.LoadScene("basePlayerScene");
    }

    public void Leaderboards()
    {
        //TODO this should show the leader board I guess
    }


    public void Lose()
    {
        Score._instance.gameObject.SetActive(false);
        score.text = "Score          " + PlayerProfile.Instance.LatestScore;
        newHighscore = PlayerProfile.Instance.CheckForHighscore();
        highscore.text = "HighScore           " + PlayerProfile.Instance.HighScore;
        LoseMenu.SetActive(true);
        _instance = null;
    }
}
