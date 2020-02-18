using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardInputManager : MonoBehaviour
{
    
    public GameObject topLeft;
    public GameObject topRight;
    public GameObject buttomRight;
    public GameObject buttomLeft;
    public bool overrideSetting;
    public  bool isToggle;


    void Start()
    {
        topLeft.SetActive(false);
        topRight.SetActive(false);
        buttomLeft.SetActive(false);
        buttomRight.SetActive(false);
        if (!overrideSetting)
        {
            isToggle = PlayerProfile.Instance.HoldControlsOn;
        }  
    }

    // Update is called once per frame
    void Update()
    {
        if (isToggle)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                topLeft.SetActive(!topLeft.activeSelf);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                topRight.SetActive(!topRight.activeSelf);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                buttomLeft.SetActive(!buttomLeft.activeSelf);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                buttomRight.SetActive(!buttomRight.activeSelf);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                topLeft.SetActive(true);
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                topLeft.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                topRight.SetActive(true);
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                topRight.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                buttomLeft.SetActive(true);
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                buttomLeft.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                buttomRight.SetActive(true);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                buttomRight.SetActive(false);
            }
        }
    }
}
