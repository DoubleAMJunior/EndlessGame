using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    #region Inspector Atributes
    public Transform playerTransform;
    public int forceAmount;
    public bool isForceSQRT;
    public int sqrtForceMultiplier;
    #endregion


    #region Private Atributes
    private Rigidbody playerRigidbody;
    private Transform _transform;
    private Vector3 force;
    private LineRenderer _lineRenderer;
    #endregion

    #region Behaviour Functions
    private void Start()
    {
        _transform = transform;
        playerRigidbody = playerTransform.gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        force = _transform.position - playerTransform.position;
        force.z = 0;
        if (isForceSQRT)
        { 
            var tempMag = force.magnitude;
            force.Normalize();
            force *= Mathf.Sqrt(tempMag) * sqrtForceMultiplier;
        }
        force *= forceAmount;
        playerRigidbody.AddForce(force);
    }
    
    private void OnEnable()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0,gameObject.transform.position);
        _lineRenderer.SetPosition(1,playerTransform.position);
    }

    
    void Update()
    {
        _lineRenderer.SetPosition(1,playerTransform.position);
    }
    #endregion
}