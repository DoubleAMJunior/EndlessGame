#define DEBUG
#if DEBUG
using System;
using System.IO;
using UnityEngine;

public class TestDataSaver : MonoBehaviour
{
    public static TestDataSaver Instance;
    
    public  string currentObstacle;
    public  string pastObstacle;
    public int bombsGenerated;
    public int numOfNearMiss;
    private StreamWriter cout;
    //score
    public float cTime;
    //real time
    public  void add(string name)
    {
        pastObstacle = currentObstacle;
        currentObstacle = name;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cTime = Time.time;
    }

    public  void saveData(bool byCollision)
    {
        if (cout==null)
        {
            string tempPath = Path.Combine(Application.persistentDataPath, "TestData");
            tempPath = Path.Combine(tempPath, "testFile" + ".txt");
            if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
            }
            cout = File.AppendText(tempPath);
            print(tempPath);
        }

        try
        {
            cout.Write("dead by bomb:"+!byCollision
                                      +" currentObstacle:"+ currentObstacle
                                      +" pastObstacle:"+pastObstacle
                                      +" number of bombs generated:"+bombsGenerated
                                      +" number of near miss:"+numOfNearMiss
                                      +" score:"+Score._instance.score
                                      +" time:"+(Time.time-cTime)
                                      +" realLifeTime:"+DateTime.Now
                                      +"\n");
            print("dead by bomb:"+!byCollision
                                 +" currentObstacle:"+ currentObstacle
                                 +" pastObstacle:"+pastObstacle
                                 +" number of bombs generated:"+bombsGenerated
                                 +" number of near miss:"+numOfNearMiss
                                 +" score:"+Score._instance.score
                                 +" time:"+(Time.time-cTime)
                                 +" realLifeTime:"+DateTime.Now
                                 +"\n");
            cout.Flush();

        }
        catch (Exception e)
        {
            print(e.StackTrace);
        }
        
    }
}
#endif
