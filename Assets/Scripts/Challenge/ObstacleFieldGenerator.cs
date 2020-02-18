using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleFieldGenerator : MonoBehaviour
{
    public static ObstacleFieldGenerator instance;

    #region InspectorAtributes
    public MovingEnviroment startGenerationField;
    public GameObject[] easyPrefabsRefrences;
    public GameObject[] mediumPrefabsRefrences;
    public GameObject[] hardPrefabsRefrences;
    public List<GameObject>[] pool = new List<GameObject>[3]; //from easy to hard by number increament
    #endregion
    [HideInInspector] public Vector2 difficulty;
    private MovingEnviroment latestField;
    private int preLoadInstantiateSleepTime = 1; 

    #region SetupFunctions
    private void Awake()
    {
        instance = this;
        latestField = startGenerationField;
        for (int i = 0; i < 3; i++)
        {
            pool[i] = new List<GameObject>();
        }

        StartCoroutine(loadInPrefabs());
    }

    private void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            CreateNew();
        }
    }

    IEnumerator loadInPrefabs()
    {
        yield return LoadFromRefrenceToScene(easyPrefabsRefrences,
            pool[0], 7);
        yield return LoadFromRefrenceToScene(mediumPrefabsRefrences,
            pool[1], -1);
        yield return LoadFromRefrenceToScene(hardPrefabsRefrences,
            pool[2], -1);
    }

    public IEnumerator LoadFromRefrenceToScene(GameObject[] refrence,List<GameObject> pool,int preLoadNumber)
    {
        for (int i=0;i<refrence.Length;i++)
        {
            GameObject go = Instantiate(refrence[i]);
            pool.Add(go);
            go.SetActive(false);
            go = Instantiate(refrence[i]);
            pool.Add(go);
            go.SetActive(false);
            if (i>preLoadNumber)
            {
                yield return new WaitForSeconds(preLoadInstantiateSleepTime);
            }
        }
        
    }  
    #endregion

    #region Behaviour Functions
    public void CreateNew()
    {
        float randNum = Random.Range(0, 1f);
        int poolnum;

        if (difficulty.x > difficulty.y)
        {
            float tmp = difficulty.x;
            difficulty.x = difficulty.y;
            difficulty.y = tmp;
        }

        if (randNum < difficulty.x)
        {
            poolnum = 0;
        }
        else if (randNum < difficulty.y)
        {
            poolnum = 1;
            if (pool[poolnum].Count<1)
            {
                poolnum = 0;
            }
        }
        else
        {
            poolnum = 2;
            if (pool[poolnum].Count<1)
            {
                poolnum = 1;
            }
            if (pool[poolnum].Count<1)
            {
                poolnum = 0;
            }
            
        }

        int index;
        int count = pool[poolnum].Count;
        if (count == 0)
        {
            index = 0;
            poolnum--;
        }

        index = Random.Range(0, count - 1);



        GameObject go = pool[poolnum][index];
        PlaceObstacle(go);
        pool[poolnum].Remove(go);
    }

    public void PlaceObstacle(GameObject go)
    {
        var latestTransform = latestField.transform;
        var latestPos = latestTransform.position;
        go.transform.position = new Vector3(latestPos.x, latestPos.y, 0);
        var obs = go.GetComponent<MovingEnviroment>();
        var startPos = obs.startTransform.position;
        var endPos = obs.endTransform.position;
        var z = latestPos.z + (latestField.endTransform.position.z - latestPos.z) +
                (go.transform.position.z - startPos.z);
        go.transform.position = new Vector3(latestPos.x, latestPos.y, z);
        latestField = go.GetComponent<MovingEnviroment>();
        go.SetActive(true);
        
        //Setup ObstacleVisibility for the current last obstacle
        ObstacleVisibility[] df = go.transform.GetComponentsInChildren<ObstacleVisibility>();
        for (int i = 0; i < df.Length; i++)
        {
            df[i].enabled = true;
        }
    }

    public void ReturnToPool(GameObject movingEnviromentGameObject)
    {
        if (movingEnviromentGameObject.CompareTag("Easy"))
        {
            pool[0].Add(movingEnviromentGameObject);

        }
        else if (movingEnviromentGameObject.CompareTag("Normal"))
        {
            pool[1].Add(movingEnviromentGameObject);
        }
        else
        {
            pool[2].Add(movingEnviromentGameObject);
        }

       movingEnviromentGameObject.SetActive(false);
    }
    
    
    #endregion
}
