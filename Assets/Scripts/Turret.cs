using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject topTurret;
    [SerializeField] private GameObject missile;
    [SerializeField] private Transform[] missileSpawnPositions;
    [SerializeField] private GameObject lockSprite;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float lockdown = 2f;
    [SerializeField] private float cooldown = 5f;

    private TurretSight sight;
    private bool canFire = false;
    private bool lockSpriteEnabled = false;
    
    private void Awake()
    {
        sight = GetComponentInChildren<TurretSight>();
        sight.OnPlayerOnSight += Fire;
        sight.OnPlayerLostSight += DismissFire;
    }

    private void Update()
    {
        if(sight.Target != null)
        {
            Quaternion lookRotation = Quaternion.LookRotation(sight.Target.transform.position - topTurret.transform.position);
            topTurret.transform.rotation =  Quaternion.Slerp(topTurret.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        if(lockSpriteEnabled)
            lockSprite.transform.LookAt(Camera.main.transform.position, Vector3.up);
    }

    public void Fire()
    {
        canFire = true;
        StartCoroutine(FireRate());
    }

    public void DismissFire()
    {
        canFire = false;
    }

    public void Damage()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        explosion.Play();
        Destroy(gameObject,1f);
    }

    public void EnableLockSprite(bool enabled)
    {
        lockSpriteEnabled = enabled;
        lockSprite.SetActive(enabled);
    }

    public GameObject GameObject
    {
        get => gameObject;
    }

    private IEnumerator FireRate()
    {
        yield return new WaitForSecondsRealtime(lockdown);
        while(canFire)
        {
            ShotMissiles();
            yield return new WaitForSecondsRealtime(cooldown);
        }
    }

    private void OnDestroy()
    {
        sight.OnPlayerOnSight -= Fire;
        sight.OnPlayerOnSight -= DismissFire;
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
}    