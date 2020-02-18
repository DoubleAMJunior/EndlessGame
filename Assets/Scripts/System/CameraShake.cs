using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = Camera.main.GetComponent<CameraShake>();
            }

            return _instance;
        }
    }

    #region Inspector Atributes
    public AnimationCurve curve;
    public float maxShakeIntensity;
    public float minShakeIntensity;
    public float maxAptitude;
    public float minAptitude;
    public float maxShakeTime;
    #endregion

    #region Code Public Atributes
    [HideInInspector]
    public float shakeIntensity;
    [HideInInspector]
    public float Aptitude;
    #endregion

    #region Private Atributes
    private static CameraShake _instance;
    private Vector3 shakePos;
    private Vector3 shakeDir;
    private Vector3 origin;
    #endregion

    private void Awake()
    {
        origin = transform.position;
    }

    public IEnumerator shake()
    {
       float timer =0;
       
        while (timer<=maxShakeTime)
        {
            timer += Time.deltaTime;

            float magnitude = Mathf.Lerp(0, 1, timer / maxShakeTime);
            
            var xPos = (magnitude) * shakeIntensity;
            var yPos = (magnitude) * shakeIntensity + 100;

            shakePos = new Vector3((Mathf.PerlinNoise(xPos, 1) - 0.5f) * Aptitude
                           , (Mathf.PerlinNoise(yPos, 1) * Aptitude), 0) * curve.Evaluate(magnitude);
            
            
            transform.position = origin + shakePos;
            
            yield return null;
        }

        transform.position = origin;
    }

}
