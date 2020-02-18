#define DEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPoolManager : MonoBehaviour
{
   public static BombPoolManager _instance;

   #region InspectorAtributes
   public Queue<GameObject> bombPool;
   public Transform SceneBombsParent;
   public float generatorSleepAmount;
   public float bombGenerateProbability;
   public float initialSleepTime = 20;
   [Range(0,1)]public float chanceOf4Bombs;
   [Range(0,1)]public float chanceOf3Bombs;
   [Range(0,1)]public float chanceOf2Bombs;
   [Range(0,1)]public float chanceOf1Bomb;
   
   #endregion
 
   #region PrivateAtributes
   [HideInInspector]
   public IEnumerator mainRoutine;
   private readonly Vector2 verticalWalls=new Vector2(7.75f,-7.75f);
   private readonly Vector2 horizentalWalls=new Vector2(4.25f,-4.25f);
   private const float startDepth=150f;
   #endregion

   #region SetupFunctions 
   private void Awake()
   {
      _instance = this;
   }
   private void Start()
   {
      bombPool=new Queue<GameObject>(); 
      for (int i=0;i<SceneBombsParent.childCount;i++)
      {
         GameObject bomb = SceneBombsParent.GetChild(i).gameObject;
         bombPool.Enqueue(bomb);
         bomb.SetActive(false);
      }

      StartCoroutine(mainRoutine=BombGenerator());
   }
   #endregion

   #region Behaviour Functions 
   IEnumerator BombGenerator()
   {
      yield return new WaitForSeconds(initialSleepTime);
      while (true)
      {
         float random = Random.Range(0, 1f);
         if (random >1-bombGenerateProbability)
         {
#if DEBUG
            TestDataSaver.Instance.bombsGenerated++;   
#endif
            random = Random.Range(0f, 1f);
            float probability = 1f;
            if (random > (probability-=chanceOf4Bombs))
            {
               CreateNewBomb(3);
               CreateNewBomb(2);
               CreateNewBomb(1);
               CreateNewBomb(0);
            }
            else if (random >(probability-=chanceOf3Bombs))
            {
               CreateNewBomb(Random.Range(1, 2));
               CreateNewBomb(Random.Range(0, 1));
               CreateNewBomb(Random.Range(2, 3));
            }
            else if (random > (probability-=chanceOf2Bombs))
            {
               CreateNewBomb(Random.Range(1, 2));
               CreateNewBomb(Random.Range(2, 3));
               

            }
            else if (random > (probability-=chanceOf1Bomb))
            {
               CreateNewBomb(Random.Range(0, 1));
            }
            
         }
         yield return new WaitForSeconds(generatorSleepAmount);
      }
   }

   public void CreateNewBomb(int direction)
   {
      
      if (bombPool.Count<1)
      {   
         return;
      }
      Vector3 generationPos=getPositionToSpawn(direction);
      GameObject bomb = bombPool.Dequeue();
      bomb.transform.position = generationPos;
      bomb.SetActive(true);

   }

   public Vector3 getPositionToSpawn(int direction)
   {
      Vector3 generationPos;
      //-1 for fully random then clockwise from 0to 3 for up to left for last;
      if (direction == -1) 
      {
         float rand = Random.Range(0, 1f);
         if (rand>0.5f)
         {
            rand = Random.Range(0, 1f);
            float xpos;
            if (rand>0.5f)
            {
               xpos = verticalWalls.x;
            }
            else
            {
               xpos = verticalWalls.y;
            }
            generationPos=new Vector3(xpos,Random.Range(horizentalWalls.x,
               horizentalWalls.y),startDepth);
         }
         else
         {
            rand = Random.Range(0, 1f);
            float ypos;
            if (rand>0.5f)
            {
               ypos = horizentalWalls.x;
            }
            else
            {
               ypos = horizentalWalls.y;
            }
            generationPos=new Vector3(Random.Range(verticalWalls.x,
               verticalWalls.y),ypos,startDepth);
         }
   
      }
      else if(direction==0)
      {
         generationPos=new Vector3(Random.Range(verticalWalls.x,
            verticalWalls.y),horizentalWalls.x,startDepth);
      }
      else if(direction==1)
      {
         generationPos=new Vector3(verticalWalls.y,Random.Range(horizentalWalls.x,
            horizentalWalls.y),startDepth);
      }
      else if(direction==2)
      {
         generationPos=new Vector3(Random.Range(verticalWalls.x,
            verticalWalls.y),horizentalWalls.y,startDepth);
      }
      else
      {
         generationPos=new Vector3(verticalWalls.x,Random.Range(horizentalWalls.x,
            horizentalWalls.y),startDepth);
      }

      return generationPos;
   }
#endregion
}
