using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatedDoorMove : MonoBehaviour
{
    #region Inspector Atributes 
    public float waitTime;
    public float moveSpeed;
    public Vector3 offsetVector;
    #endregion

    #region Private Atributes
    private IEnumerator moveRoutine;
    private Vector3 _startPos;
    private Vector3 _secondPos;
    private Transform _transform;
    private Vector3 tempTransform;
    #endregion

    #region setup Functions 
    private void OnEnable()
    {
        tempTransform = transform.localPosition;
        offsetVector.z = 0;
        _startPos = new Vector3(transform.localPosition.x,transform.localPosition.y ,transform.localPosition.z);
        _secondPos = _startPos + offsetVector;
        _transform = transform;
        StartCoroutine(AutoMove());    
    }
    private void OnDisable()
    {
        transform.localPosition = tempTransform;
    }
#endregion
    private IEnumerator AutoMove()
    {
        while (true)
        {
            while (Vector3.Distance(_transform.localPosition, _secondPos) > 0.1)
            {
                var temp = _transform.localPosition;
                temp += (_secondPos - _startPos).normalized * moveSpeed * Time.deltaTime;
                temp.z = _transform.localPosition.z;
                _secondPos.z = temp.z;
                _startPos.z = temp.z;
                _transform.localPosition = temp;
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
            while (Vector3.Distance(_transform.localPosition, _startPos) > 0.1)
            {
                var temp = _transform.localPosition;
                temp += (_startPos - _secondPos).normalized * moveSpeed * Time.deltaTime;
                temp.z = _transform.localPosition.z;
                _secondPos.z = temp.z;
                _startPos.z = temp.z;
                _transform.localPosition = temp;
                yield return null;
            }
            
            yield return new WaitForSeconds(waitTime);
        }
    }
}
