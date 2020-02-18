#define DEBUG
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mime;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Inspector Atributes
    public bool godMode;
    [Space] public GameObject dangerIndicatorLight;
    public LayerMask dangerousObstaclesLayer;
    public float dangerDetectorRange;
    public Color DangerColor;
    public Color SafeColor;
    public float detectorDangerColorSpeed;
    public float detectorSafeColorSpeed = 0.5f;
    public float playerColliderRadius;
    [Space] public Transform[] playerBilbilak;
    [Space] public GameObject wallHitPaticlePrefab;
    public float wallHitParticleScaleMultiplier;
    [Space] public bool shakeEnable;
    [Space] public float maxShakeImpulse = 100f;
    #endregion

    #region Private Atributes
    private Vector3 particleminScale = new Vector3(0.01f, 0.01f, 0.01f);
    private Quaternion rotator;
    private float minShakeImpulse = 25f;
    private Material playerMaterial;
    #endregion

    #region Behaviour Functions
    private void Start()
    {
        playerMaterial = dangerIndicatorLight.GetComponent<MeshRenderer>().material;
        rotator = new Quaternion();
    }

    private void OnTriggerEnter(Collider other)
    {
//        else if (other.CompareTag("obstacle") && godMode)
//        {
//            print("FaggotLoser");
//        }
        if (other.CompareTag("obstacle") && !godMode)
        {
            lose(true);
#if DEBUG
            TestDataSaver.Instance.add(other.transform.parent.name);
#endif
        }

    }

    //DangerLightHandler
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, playerColliderRadius, transform.forward, out hit, dangerDetectorRange, dangerousObstaclesLayer))
            playerMaterial.color = Color.Lerp(SafeColor, DangerColor,
                detectorDangerColorSpeed * ((dangerDetectorRange - hit.distance) / dangerDetectorRange));
        else
            playerMaterial.color =
                Color.Lerp(playerMaterial.color, SafeColor, detectorDangerColorSpeed * detectorSafeColorSpeed);

        for (int i = 0; i < RefrenceHolder._instance.Anchors.Length; i++)
        {
            Vector3 direction = RefrenceHolder._instance.Anchors[i].transform.position - transform.position;
            direction = direction.normalized;
            playerBilbilak[i].position = transform.position + direction * playerColliderRadius;
            playerBilbilak[i].rotation = Quaternion.LookRotation(playerBilbilak[i].forward, direction);
            Transform laser = RefrenceHolder._instance.Anchors[i].transform.GetChild(0);
            laser.rotation=Quaternion.LookRotation(laser.forward, direction*-1);
        }

    }
    //wall hit
    private void OnCollisionEnter(Collision other)
    {
        float impulse = other.impulse.magnitude;
       
        spawnWallHitParticle(impulse,other);
        
        VibrationFunction(impulse);

        //screen shake
        if (shakeEnable && !other.gameObject.CompareTag("Bomb"))
        {
            float shakeAmount = impulse / maxShakeImpulse;
            
           
           // ShakeCamera(shakeAmount,other);
           CameraShake.Instance.shakeIntensity = Mathf.Lerp(CameraShake.Instance.minShakeIntensity,
               CameraShake.Instance.maxShakeIntensity,shakeAmount);

           CameraShake.Instance.Aptitude = Mathf.Lerp(CameraShake.Instance.minAptitude,
               CameraShake.Instance.maxAptitude, shakeAmount);
           
     
           StartCoroutine(CameraShake.Instance.shake());
           
        }
    }

    public void VibrationFunction(float impulse)
    {
        if (impulse > maxShakeImpulse)
        {
//            StartCoroutine(vibrate(0.5f));
            androidVibrate(200);
        }
        else if (impulse > minShakeImpulse)
        {
            //     StartCoroutine(vibrate(0.1f));
            androidVibrate(100);
        }
    }
    
    public void spawnWallHitParticle(float impulse,Collision other)
    {
        rotator = Quaternion.FromToRotation(wallHitPaticlePrefab.transform.forward, other.transform.forward);
        rotator = wallHitPaticlePrefab.transform.rotation * rotator;
        Vector3 spawnPoint=transform.position;
        
        if (other.gameObject.tag.Equals("BottomWall")
            || other.gameObject.tag.Equals("TopWall"))
        {
            spawnPoint.y = other.transform.position.y;
        }
        else if (other.gameObject.tag.Equals("RightWall")
                 || other.gameObject.tag.Equals("LeftWall"))
        {
            spawnPoint.x = other.transform.position.x;
        }
       
       
            
        GameObject go = Instantiate(wallHitPaticlePrefab, transform.position, rotator);
        Vector3 setScale = impulse * wallHitParticleScaleMultiplier * particleminScale;
        go.transform.localScale = setScale;
        MoveSystem.instance.subscribe(go.transform);
        Destroy(go, 2);   
    }
   #endregion

    #region General Functions
   void androidVibrate(int timeInMiliSec)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        AndroidJavaClass unity= new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
        AndroidJavaObject ca = unity.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass vibratorClass = new AndroidJavaClass("android.os.Vibrator"); 
        AndroidJavaObject vibratorService = ca.Call<AndroidJavaObject>("getSystemService", "vibrator");

        vibratorService.Call("vibrate", (long)timeInMiliSec);
        unity.Dispose();
        ca.Dispose();
        vibratorClass.Dispose();
        vibratorService.Dispose();
    }
    
    IEnumerator DeviceVibrate(float secconds)
    {
        float passed = 0;
        while (passed<secconds)
        {
            passed += Time.deltaTime;
            Handheld.Vibrate();
            //print("Shaking");
            yield return null;
        }
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.color=Color.blue;
        Gizmos.DrawSphere(transform.position,ObjectRadius);
        Gizmos.DrawRay(transform.position,transform.forward*maxDangerDist);
    }*/

    public void lose(bool byCollision)
    {
        if (godMode)
        {
            return;
        }
        if (GameManager.Instance.lose)
        {
            return;
        }

        if (NearMiss.startNearMiss!=null)
        {
            StopCoroutine(NearMiss.startNearMiss);
        }
#if DEBUG
        TestDataSaver.Instance.saveData(byCollision);
#endif
        
        transform.DOMove(new Vector3(transform.position.x,transform.position.y,-3.5f), 0.3f, false);
        //Disable all bombs
        GameObject[] go = GameObject.FindGameObjectsWithTag("Bomb");
        for (int i = 0; i < go.Length; i++)
        {
            go[i].SetActive(false);
        }
        //pass to gameManager for Lose
        GameManager.Instance.lose = true;
        GameManager.Instance.startLoseSequence(gameObject,byCollision);
        
    }
    #endregion
}