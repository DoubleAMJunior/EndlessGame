using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Bomb : MonoBehaviour
{
    
    #region InspectorAtributes
    public Color BombSafeColor;
    public Color BombDangerColor;
    [SerializeField] private AudioClip beepSound;
    [Space][SerializeField]private float attackStartPoint;
    [SerializeField]private float bombTimer;
    [Space][SerializeField]private float chasePrecision;
    [SerializeField]private float chaseSpeed;
    [SerializeField]private float maxChaseTime;
    [Space] [SerializeField] private float connectionDist;
    #endregion

    #region PrivateAtributes
    private const float minChaseTime=0.5f;
    private float timeInChase;
    private Transform _transform;
    private GameObject _player;
    private Transform _playerTransform;
    private Rigidbody _playerRigidBody;
    private IEnumerator _explosionRoutine;
    private FixedJoint _joint;
    //Beep Code
    private AudioSource _audioSource;
    private IEnumerator _beepRoutine;
    private MeshRenderer mr;
    #endregion

    #region Pool And Setup Functions
    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _player.GetComponent<Transform>();
        _playerRigidBody = _player.GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        _explosionRoutine = Explosion();
        GetComponent<Rigidbody>().velocity=Vector3.zero;
        _playerRigidBody.useGravity = false;
        _transform = transform;
        mr.material.color = BombSafeColor;
        StartCoroutine(moveBomb());
        //Beep Code
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = beepSound;
        _beepRoutine = MakeBeep();
    }
    void BombDestroy()
    {
        gameObject.SetActive(false);
        BombPoolManager._instance.bombPool.Enqueue(gameObject);
    }
    private void OnDisable()
    {
        MoveSystem.instance.unSubscribe(transform);
    }
    #endregion

    #region Behaviour Functions 
    private void OnCollisionEnter(Collision other)//hit player and the walls around
    {
        if (other.gameObject.CompareTag("TopWall") || other.gameObject.CompareTag("LeftWall") ||
            other.gameObject.CompareTag("RightWall") || other.gameObject.CompareTag("BottomWall"))
        {
            if (_joint)
            {
                Destroy(_joint);
                StartCoroutine(BombDefused());
                Destroy(_joint);
                StopCoroutine(_explosionRoutine);
                StartCoroutine(BombDefused());

                
            }
            else if(timeInChase>minChaseTime)
            {
                BombDestroy();
            }
        }   
    }
    private void OnTriggerEnter(Collider other)//hit the obstacles
    {
        if (other.gameObject.CompareTag("obstacle"))
        {
              if (_joint)
              {
                  Destroy(_joint);
                  StartCoroutine(BombDefused());
                  Destroy(_joint);
                  StopCoroutine(_explosionRoutine);
                  StartCoroutine(BombDefused());

                
              }
              else if(timeInChase>minChaseTime)
              {
                  StartCoroutine(BombDefused());
              }
              else
              {
                  BombDestroy();
              }
        }
    }
    IEnumerator moveBomb()
    {
        if (MoveSystem.instance==null)
        {
            yield return new WaitForSeconds(1);
            print(MoveSystem.instance);
        }
        MoveSystem.instance.subscribe(transform);
        while (_transform.position.z > attackStartPoint)
        {
            yield return new WaitForFixedUpdate();
        }
        MoveSystem.instance.unSubscribe(transform);
        Vector3 targetNormalDirection=_playerTransform.position-_transform.position;
        targetNormalDirection = targetNormalDirection.normalized;
        timeInChase = 0;  
        while (Vector3.Distance(_playerTransform.position,_transform.position)>connectionDist)
        {
            transform.Translate(targetNormalDirection * chaseSpeed);     
            if (timeInChase<maxChaseTime)
            {
                targetNormalDirection=
                    Vector3.Lerp(targetNormalDirection,
                        (_playerTransform.position-_transform.position).normalized,
                        chasePrecision);
                timeInChase += Time.deltaTime;
            }
            yield return new WaitForFixedUpdate();
        }
        reachedPlayer();
    }
    void reachedPlayer()
    {
        _joint = gameObject.AddComponent<FixedJoint>();
        _joint.connectedBody = _playerRigidBody;
        StartCoroutine(_explosionRoutine = Explosion());
        //Beep Code
        StartCoroutine(_beepRoutine);
    }
    IEnumerator BombDefused()
    {
        MoveSystem.instance.subscribe(transform);
        while (_transform.position.z > -20)
        {
            yield return new WaitForFixedUpdate();
        }
        MoveSystem.instance.unSubscribe(transform);
        BombDestroy();
    }
    IEnumerator Explosion()
    {
        float remainingTime = bombTimer;
        while (remainingTime >= 0)
        {
            remainingTime -= 0.1f;
            mr.material.color= Color.Lerp(BombDangerColor, BombSafeColor, remainingTime / bombTimer);   
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine(_beepRoutine);
        _player.GetComponent<Player>().lose(false);
    }
    IEnumerator MakeBeep()
    {
        float remainingTime = bombTimer;
        while (remainingTime >= 0)
        {
            var delay = remainingTime / 3;
            if (delay >= 0.3)
            {
                remainingTime -= delay;
            }
            
            _audioSource.Play();
            transform.DOShakeScale(0.1f, 0.5f);
            yield return new WaitForSeconds(delay);    
        }
    }
    #endregion
}
