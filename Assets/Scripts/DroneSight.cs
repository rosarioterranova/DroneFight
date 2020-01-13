using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSight : MonoBehaviour
{
    public Action<IEnemy> OnEnemyOnSight;
    public Action<IEnemy> OnEnemyLostSight;

    private void OnTriggerEnter(Collider other)
    {
        IEnemy enemy = other.GetComponent<IEnemy>();
        if(enemy != null)
        {
            OnEnemyOnSight?.Invoke(enemy);
            enemy.EnableLockSprite(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IEnemy enemy = other.GetComponent<IEnemy>();
        if(enemy != null)
        {
            OnEnemyLostSight?.Invoke(enemy);
            enemy.EnableLockSprite(false);
        }
    }
}
