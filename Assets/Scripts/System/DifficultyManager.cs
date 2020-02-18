using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    #region InspectorAtributes
    public int difficultyLvl = 0;
    [SerializeField]private int rampUpTime = 10;
    [SerializeField]private List<Vector2> obstacleGeneratorDificultySettingList;
    public List<float> speedList;
    public static float scoreMultiplier;
    #endregion
    
    private IEnumerator ie;
    
    private void Start()
    {
        ie = RampItUp();
        StartCoroutine(ie);
        ObstacleFieldGenerator.instance.difficulty = obstacleGeneratorDificultySettingList[difficultyLvl];
    }

    private IEnumerator RampItUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(rampUpTime);
            if(difficultyLvl < 9)
                difficultyLvl++;
            ObstacleFieldGenerator.instance.difficulty = obstacleGeneratorDificultySettingList[difficultyLvl];
            MoveSystem.instance.changeSpeed(speedList[difficultyLvl]);
            scoreMultiplier = speedList[difficultyLvl];
        }
    }

    
}
