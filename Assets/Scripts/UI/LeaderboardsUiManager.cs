﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardsUiManager : MonoBehaviour
{
   public GameObject mainMenu;

   public void Back()
   {
      mainMenu.SetActive(true);
      gameObject.SetActive(false);
   }
   
}
