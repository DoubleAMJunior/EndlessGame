using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance=GameObject.Find("GameManager").GetComponent<GameManager>(); 
     
            }
            return _instance;
        }
    }

    

    #region Inspector Atributes
    public int targetFPS;
    public GameObject destructionParticle;
    public GameObject destroyedPlayer;
    public float destructionForce;
    public  float maxDeathAnimationTime;
    #endregion
    
    private static GameManager _instance;
    [HideInInspector] public bool lose =false;
    private float minDeathAnimationTime=0.5f;
    private const float radiusOfBlast = 4;
    
    
    private void Start()
    {
        Application.targetFrameRate = targetFPS;
//TODO        AudioManager._instance.SetMuteState(1,false);
    }
    
  
    public IEnumerator LoseAnimationRoutine(GameObject player,bool byCollision)
    {
        player.gameObject.GetComponent<Rigidbody>().velocity=new Vector3(0,0,0);
        if (byCollision)
        {  
            Player pScriptRef = player.GetComponent<Player>();
            pScriptRef.enabled = false;
            player.GetComponent<MeshRenderer>().material.color = pScriptRef.DangerColor;
        }

        for (int i = 0; i < RefrenceHolder._instance.Anchors.Length; i++)
        {
            RefrenceHolder._instance.Anchors[i].SetActive(false);
        }

        GameObject inputManager = GameObject.Find("InputManager");
        inputManager.GetComponent<TouchManager>().enabled = false;
        GetComponent<KeyBoardInputManager>().enabled = false;

        MoveSystem.instance.enabled = false;

        BombPoolManager._instance.StopCoroutine(BombPoolManager._instance.mainRoutine);
        BombPoolManager._instance.enabled = false;
        
        Instantiate(destructionParticle,player.transform.position,Quaternion.identity);
        
        
        CameraShake.Instance.Aptitude = 2;
        CameraShake.Instance.shakeIntensity = 100;
        CameraShake.Instance.maxShakeTime = 1.5f;
        StartCoroutine(CameraShake.Instance.shake());
        
        
        yield return new WaitForSeconds(1.5f);
        GameObject destroyed =
            Instantiate(destroyedPlayer, player.transform.position, Quaternion.identity);
        if (byCollision)
        {
            GameObject dangerLight= destroyed.transform.Find("white_plate").gameObject;
            dangerLight.GetComponent<MeshRenderer>().material.color=player.GetComponent<MeshRenderer>().material.color;
        }
        Rigidbody[] partsPhysics=destroyed.transform.GetComponentsInChildren<Rigidbody>();
        player.SetActive(false);
        
        for (int i = 0; i < partsPhysics.Length; i++)
        {
            partsPhysics[i].AddExplosionForce(destructionForce, player.transform.position
            ,radiusOfBlast);
        }
        
    }
    public IEnumerator LoseRoutine()
    {
        AudioManager._instance.SetMuteState(1,true);
        float timer = 0;
        while (timer < maxDeathAnimationTime)
        {
            if (Input.touchCount > 0 || Input.anyKey && timer>=minDeathAnimationTime)
            {
                timer += maxDeathAnimationTime;
            }

            timer += Time.deltaTime;
            
            yield return null;
        }
        RefrenceHolder._instance.eventSystem.SetActive(true);
        LoseMenuUiManager.Instance.Lose();

    }

    public void startLoseSequence(GameObject player,bool byCollision)
    {
        StartCoroutine(GameManager.Instance.LoseAnimationRoutine(player,byCollision));
        StartCoroutine(GameManager.Instance.LoseRoutine());
    }
}
