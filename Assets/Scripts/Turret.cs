﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IEnemy
{

    public Action OnTurretDestroyed;

    [SerializeField] private GameObject topTurret;
    [SerializeField] private GameObject missile;
    [SerializeField] private Transform missileSpawnLocation;
    [SerializeField] private GameObject lockSprite;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float lockdown = 2f;
    [SerializeField] private float cooldown = 5f;

    private TurretSight sight;
    private bool canFire = false;
    private bool lockSpriteEnabled = false;
    private AudioSource audioSource;
    private DroneController droneController;
    private bool rewindEnabled = false;
    private bool hitted = false;
    
    private void Awake()
    {
        sight = GetComponentInChildren<TurretSight>();
        sight.OnPlayerOnSight += Fire;
        sight.OnPlayerLostSight += DismissFire;
        audioSource = GetComponent<AudioSource>();
        droneController = FindObjectOfType<DroneController>(); //TODO fix ref
        DroneController.OnRewindStart += () => rewindEnabled = true;
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
        if(hitted) return;
        hitted = true;
        var renderMeshes = GetComponentsInChildren<MeshRenderer>();
        Array.ForEach(renderMeshes, render => render.enabled = false);
        explosion.Play();
        audioSource.Play();
        StartCoroutine(RewindCheckpoint());
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
        GameObject missileObj = Instantiate(missile, missileSpawnLocation.position,Quaternion.identity);
        missileObj.GetComponent<Missile>().target = droneController.transform;
    }

    IEnumerator RewindCheckpoint()
    {
        yield return new WaitForSecondsRealtime(droneController.RewindDuration);
        if(!rewindEnabled)
        {
            OnTurretDestroyed?.Invoke();
            Destroy(gameObject,1f);
        }
        else
        {
            var renderMeshes = GetComponentsInChildren<MeshRenderer>();
            Array.ForEach(renderMeshes, render => render.enabled = true);
            rewindEnabled = false;
            hitted = false;
        }
    }
}    