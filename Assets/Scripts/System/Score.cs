using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score _instance;
    #region publicAtributes
    public float  score = 0;
    public  float scoreSteps;
    public bool isPlaying;
    #endregion
    private TextMeshProUGUI text;

    #region Setup Functions 
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(scoreUpdate());
    }
#endregion

    #region Behaviour Functions 
    public IEnumerator ScoreFlash(float maxSize)
    {
        isPlaying = true;
        float waitTime=0.003f;
        float delta=0.5f;
        float minSize = text.fontSize;
        while (text.fontSize<maxSize)
        {
            text.fontSize += delta;
            yield return new WaitForSeconds(waitTime);
        }

        while (text.fontSize>minSize)
        {
            text.fontSize -= delta;
            yield return new WaitForSeconds(waitTime);
        }

        isPlaying = false;
    }
    IEnumerator scoreUpdate()
    {
        while (true)
        {
            print("working");
            yield return new WaitForSeconds(0.1f);
            score += scoreSteps * DifficultyManager.scoreMultiplier;
            text.text = ((int)score).ToString();
            PlayerProfile.Instance.LatestScore = (int) score;
            
        }
    }
    #endregion
}
