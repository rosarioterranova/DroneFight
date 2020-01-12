using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject topTurret;

    private float rotationSpeed = 10f;
    private TurretSight turretSight;
    private float lockdown = 2f;
    private float cooldown = 5f;
    private bool canFire = false;
    
    private void Awake()
    {
        turretSight = GetComponentInChildren<TurretSight>();
        turretSight.OnPlayerOnSight += StartFire;
        turretSight.OnPlayerLostSight += DismissFire;
    }

    private void Update()
    {
        if(turretSight.Target != null)
        {
            Quaternion lookRotation = Quaternion.LookRotation(turretSight.Target.transform.position - topTurret.transform.position);
            topTurret.transform.rotation =  Quaternion.Slerp(topTurret.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void StartFire()
    {
        canFire = true;
        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        yield return new WaitForSecondsRealtime(lockdown);
        while(canFire)
        {
            Debug.Log("Turret fire!");
            yield return new WaitForSecondsRealtime(cooldown);
        }
    }

    private void DismissFire()
    {
        canFire = false;
        Debug.Log("Dismissed Fire");
    }

    private void OnDestroy()
    {
        turretSight.OnPlayerOnSight -= StartFire;
        turretSight.OnPlayerOnSight -= DismissFire;
    }
}    