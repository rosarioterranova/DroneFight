using System;
using UnityEngine;

public class TurretSight : MonoBehaviour
{
    public GameObject Target { get => target;}
    public Action OnPlayerOnSight;
    public Action OnPlayerLostSight;

    private GameObject target = null;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            target = other.gameObject;
            OnPlayerOnSight?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            target = null;
            OnPlayerLostSight?.Invoke();
        }
    }
}
