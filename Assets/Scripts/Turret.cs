using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject topTurret;
    [SerializeField] private GameObject missile;
    [SerializeField] private Transform[] missileSpawnPositions;
    [SerializeField] GameObject targetSprite;

    private float rotationSpeed = 10f;
    private TurretSight turretSight;
    private float lockdown = 2f;
    private float cooldown = 5f;
    private bool canFire = false;
    private bool targetSpriteEnabled = false;
    
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

        if(targetSpriteEnabled)
            targetSprite.transform.LookAt(Camera.main.transform.position, Vector3.up);
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
            ShotMissiles();
            yield return new WaitForSecondsRealtime(cooldown);
        }
    }

    private void DismissFire()
    {
        canFire = false;
        EnableTargetSprite(false);
    }

    private void OnDestroy()
    {
        turretSight.OnPlayerOnSight -= StartFire;
        turretSight.OnPlayerOnSight -= DismissFire;
    }

    private void ShotMissiles()
    {
        for (int i = 0; i < 4; i++)
            {
                GameObject missileObj = Instantiate(missile, missileSpawnPositions[i].position,Quaternion.identity);
                missileObj.GetComponent<Missile>().target = FindObjectOfType<DroneController>().transform;
                Destroy(missileObj,4f);
            }
    }

    public void Damage()
    {
        Destroy(gameObject);
    }

    public void EnableTargetSprite(bool enabled)
    {
        targetSpriteEnabled = enabled;
        targetSprite.SetActive(enabled);
    }
}    