using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject top;

    
    void Update()
    {
        var target = FindObjectOfType<DroneController>().transform;
        top.transform.LookAt(target);
    }
}
