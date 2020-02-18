using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private List<TouchHolder> detectedTouches;
    private List<TouchHolder>[] detectedHoldTouches;
//    from 0 to 3 in the order below
//    public  GameObject topLeft;
//    public  GameObject topRight;
//    public  GameObject buttomRight;
//    public  GameObject buttomLeft;

    delegate void TouchHandler();
    private TouchHandler currentTouchHandler;


    #region Setup Functions
    void Start()
    {
        RefrenceHolder._instance.eventSystem.SetActive(false);
        detectedTouches=new List<TouchHolder>();
        if (PlayerProfile.Instance.HoldControlsOn)
        {
           currentTouchHandler += HoldTouchHandler;
            detectedHoldTouches = new List<TouchHolder>[4];
            for (int i = 0; i < 4; i++)
            {
                detectedHoldTouches[i]=new List<TouchHolder>();
            }
        }
        else
        {
            currentTouchHandler += ToggleTouchHandler;
        }

    }

    
    void Update()
    {
       TouchUpdate();
       currentTouchHandler();
    }
    #endregion
    
    #region Hold Type Behaviour 
    public void HoldTouchHandler()
    {
        bool touchUpdateNeeded = CheckUpdateNeed();
        
        if (touchUpdateNeeded)
        {
                    HoldUpdate();
        }
    }
    public bool CheckUpdateNeed()
    {
        bool touchUpdateNeeded =false;
        for (int i = 0; i < 4; i++)
        {
            if (detectedHoldTouches[i].Count > 0)
            {
                for (int j = 0; j < detectedHoldTouches[i].Count; j++)
                {


                    if (detectedHoldTouches[i][j].touch.phase != TouchPhase.Ended)
                    {
                        if (detectedHoldTouches[i][j].touch.phase==TouchPhase.Moved)
                        {
                            int newAnchor = GetTouchAnchorPoint(detectedHoldTouches[i][j].touch.position);
                            if (newAnchor!=i && newAnchor!=-1)
                            {
                                RefrenceHolder._instance.Anchors[newAnchor].SetActive(true);
                                detectedHoldTouches[newAnchor].Add(detectedHoldTouches[i][j]);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;   
                        }  
                    } 
                    
                    detectedHoldTouches[i].Remove(detectedHoldTouches[i][j]);
                    if (detectedHoldTouches[i].Count != 0) continue;
                    touchUpdateNeeded = true;
                    RefrenceHolder._instance.Anchors[i].SetActive(false);                    
                }
            }
            else
            {
                touchUpdateNeeded = true;
            }
        }

        return touchUpdateNeeded;
            
    }
    public void HoldUpdate()
    {
        foreach (TouchHolder touchHolder in detectedTouches)
        {
            if (touchHolder.active)
            {
                int pointer = GetTouchAnchorPoint(touchHolder.touch.position);

                if (pointer != -1)
                {
                    RefrenceHolder._instance.Anchors[pointer].SetActive(true);
                    detectedHoldTouches[pointer].Add(touchHolder);
                    touchHolder.active = false;
                }
            }
        }
    }
    #endregion

    #region Toggle Type Behaviour 
    public void ToggleTouchHandler()
    {
        foreach (TouchHolder touchHolder in detectedTouches)
        {
            if (touchHolder.active)
            {
                int pointer=GetTouchAnchorPoint(touchHolder.touch.position);
                if (pointer!=-1)
                {
                    RefrenceHolder._instance.Anchors[pointer].SetActive(!RefrenceHolder._instance.Anchors[pointer].activeSelf);
                    touchHolder.active = false;
                }
            }
        }
    }
    #endregion

    #region Touch General Functions
    public int GetTouchAnchorPoint(Vector2 pos)
    {
        if (pos.x<Screen.width/2)
        {
            if (pos.y<Screen.height/2)//left down 
            {
                return 3;
            }

            else if (pos.y>(Screen.height-(Screen.height/2)))//left up
            {
                return 0;
            }
        }
        else if(pos.x>(Screen.width-(Screen.width/2)))
        {
            if (pos.y<Screen.height/2)//right down 
            {
                return 2;
            }

            else if (pos.y>(Screen.height-(Screen.height/2)))//right up
            {
                return 1;
            }
        }

        return -1;
    }

    private void TouchUpdate(){
  
        if (Input.touchCount > 0)
        {
            Touch[] touches = Input.touches;
            for (int i = 0; i < touches.Length; i++)
            {
                bool inCTouch=false;
                for (int j =0; j<detectedTouches.Count;j++)
                {
                    if (detectedTouches[j].id==touches[i].fingerId)
                    {
                        inCTouch = true;
                        detectedTouches[j].touch = touches[i];
                        detectedTouches[j].id = touches[i].fingerId;
                        if (detectedTouches[j].touch.phase==TouchPhase.Ended)
                        {
                            detectedTouches.RemoveAt(j);
                        }
                        break;
                    }
                }

                if (!inCTouch)
                {
                    detectedTouches.Add(new TouchHolder(touches[i]));
                }
            }
        }
    }
    #endregion
}


class TouchHolder
{
    private static float NULLIDENTITY=-10;
    public bool active;
    public float id;
    public  Touch touch;

    public TouchHolder()
    {
        active = false;
        id = NULLIDENTITY;
    }

    public TouchHolder(Touch t)
    {
        touch = t;
        id = t.fingerId;
        active = true;
    }
}
