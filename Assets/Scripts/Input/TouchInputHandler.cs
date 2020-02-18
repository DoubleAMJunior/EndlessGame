using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputHandler :MonoBehaviour, IPointerUpHandler,IPointerDownHandler
{
    public TouchManager tm;
    List<int>[] pointerIds=new List<int>[4];

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            pointerIds[i]=new List<int>();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        for (int i=0;i<4;i++)
        {
            if (pointerIds[i].Contains(eventData.pointerId))
            {
                pointerIds[i].Remove(eventData.pointerId);
                if (pointerIds[i].Count==0)
                {
                    RefrenceHolder._instance.Anchors[i].SetActive(false);
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        int anchor=tm.GetTouchAnchorPoint(eventData.position);
        RefrenceHolder._instance.Anchors[anchor].SetActive(true);
        pointerIds[anchor].Add(eventData.pointerId);
    }
}
