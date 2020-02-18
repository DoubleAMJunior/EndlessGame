using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DG.Tweening;

public class GapingObstacleMove : MonoBehaviour
{
    #region Inspector Atributes
    public const float squareWidth = 5;
    public float safeDistace;
    public float leftBound;
    public float rightBound;
    public float upperBound;
    public float lowerbound;
    public GameObject upperPlain;
    public GameObject lowerPlain;
    public GameObject leftPlain;
    public GameObject rightPlain;
    public float duration;
    #endregion

    #region Private Atributes
    private Vector3 randomPos;
    private Vector3 initialUpperPos;
    private Vector3 initialLowerPos;
    private Vector3 initialLeftPos;
    private Vector3 initialRightPos;
    #endregion
    void Start()
    {
        randomPos = new Vector3(Random.Range(leftBound, rightBound), Random.Range(lowerbound, upperBound));
        initialUpperPos = upperPlain.transform.position;
        initialLowerPos = lowerPlain.transform.position;
        initialLeftPos = leftPlain.transform.position;
        initialRightPos = rightPlain.transform.position;
        Vector3[] upperPath = {randomPos + new Vector3(0, safeDistace + squareWidth), initialUpperPos}; 
        Vector3[] lowerPath = {randomPos - new Vector3(0, safeDistace + squareWidth), initialLowerPos}; 
        Vector3[] leftPath = {randomPos - new Vector3(safeDistace + squareWidth, 0), initialLeftPos}; 
        Vector3[] rightPath = {randomPos + new Vector3(safeDistace + squareWidth, 0), initialRightPos};
        upperPlain.transform.DOPath(upperPath, duration).SetLoops(1000);
        lowerPlain.transform.DOPath(lowerPath, duration).SetLoops(1000);
        leftPlain.transform.DOPath(leftPath, duration).SetLoops(1000);
        rightPlain.transform.DOPath(rightPath, duration).SetLoops(1000);

    }
}
