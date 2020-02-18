using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelManager : MonoBehaviour
{
        
       public static TunnelManager instance;
       
       public MovingEnviroment lastField;
       
       private void Awake()
       {
           instance = this;
       }
       
       public void CreateNew(GameObject go)
       {
           
           var latestTransform = lastField.transform;
           var latestPos = latestTransform.position;
           go.transform.position = new Vector3(latestPos.x, latestPos.y, 0);
           
           
           var obs = go.GetComponent<MovingEnviroment>();
           var startPos = obs.startTransform.position;
           var endPos = obs.endTransform.position;
           var z = latestPos.z + (lastField.endTransform.position.z - latestPos.z) +
                                   (go.transform.position.z - startPos.z);
           
           go.transform.position = new Vector3(latestPos.x, latestPos.y, z);
           lastField = go.GetComponent<MovingEnviroment>();
       }
}
