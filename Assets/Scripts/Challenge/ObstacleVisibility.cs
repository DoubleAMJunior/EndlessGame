using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObstacleVisibility : MonoBehaviour
{
    #region Balance Atributes
    private static float startAlphaZpos = 269;
    private static float startZpos = 100;
    private static float maxColorVal = 1f;
    private static float colorChangeSpeed = 0.006f;
    private static float fadeInSpeed = 0.01f;
    private static Color StartColor= new Color(0.5f,0.5f,0.5f,0);
    public static float fadeOutSpeed=0.09f;
    #endregion

    #region Private refrences
    private MeshRenderer[] mRenderer;
    private Transform playerTransform;
    private Color temp;
    private  bool runed;
    private bool reached=false;
    #endregion
    private void Awake()
    {
        mRenderer = GetComponentsInChildren<MeshRenderer>();
        
    }

    void Start()   
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        runed = false;
        for(int i=0;i<mRenderer.Length;i++){
        mRenderer[i].material.color = StartColor;
        }
        temp = StartColor;
        reached = false;
    }

    private void FixedUpdate()
    {
//        
//        if (startZpos>transform.position.z-playerTransform.position.z)
//        {
//            if (temp.g>0)
//            {
//                temp.g = Mathf.Lerp(StartColor.g,0,(startZpos-(transform.position.z-playerTransform.position.z))/startZpos);
//                for(int i=0;i<mRenderer.Length;i++){
//                    mRenderer[i].material.color = temp; 
//                }
//            }
//        }
        if (transform.position.z < -4 && mRenderer[0].material.color.a >= 0)
        {
            if (!runed)
            {
                temp = mRenderer[0].material.color;
                runed = true;
            }
            temp.a -= fadeOutSpeed;
            for (int i = 0; i < mRenderer.Length; i++)
            {
                mRenderer[i].material.color = temp;
            }      
        }
        else if (startAlphaZpos>transform.position.z -playerTransform.position.z && !reached)
        {    
            if (temp.a<maxColorVal)
            {
                temp.a += fadeInSpeed;
                for (int i = 0; i < mRenderer.Length; i++)
                {
                    mRenderer[i].material.color = temp;
                }
            }
            else
            {
                reached = true;
            }
            
        }
    }
}
