using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform _target;
    
    Vector3 _targetOffset;

    private void Start()
    {
        _targetOffset = transform.position - _target.position;
    }
    void Update()
    {
        if (_target)
        {
            transform.position = Vector3.Lerp(transform.position, _target.position + _targetOffset, 0.1f);
        }
    }
}
