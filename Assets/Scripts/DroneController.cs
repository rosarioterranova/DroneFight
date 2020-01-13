﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public Action OnPrimaryWeaponFire;
    public Action OnSecondaryWeaponFire;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float dashMultiply = 4f;
    [SerializeField] private float timeToDoublePress = 1f;
    [SerializeField] private float rotationSpeedHorizontal = 2.0f;
    [SerializeField] private float rotationSpeedVertical = 2.0f;
    [SerializeField] private float acquireEnemyDistance = 50f;
    [SerializeField] private float cameraPitchClamp = 30f;
    [SerializeField] private GameObject missile;
    [SerializeField] private Transform[] missileSpawnLocation;

    private IEnemy enemyInRay = null;
    private IEnemy enemyInRange = null;
    private float yawRotation = 0.0f;
    private float pitchRotation = 0.0f;
    private ParticleSystem[] gunsParticles;
    private DroneSight droneSight;

    //Dash values
    private bool canDash = true;
    private float actualDashSpeedMultiply = 1f;
    private bool dashFirstButtonPressed = false;
    private bool dashReset = false;
    private float dashTimeOfFirstButton = 0;

    //Input key
    private const string horizontalAxys = "Horizontal";
    private const string verticalAxys = "Vertical";
    private const string forwardlAxys = "Forward";
    private const string fire1Button = "Fire1";
    private const string fire2Button = "Fire2";
    private const string specialButton = "Fire3";
    private const string mouseXAxys = "Mouse X";
    private const string mouseYAxys = "Mouse Y";

    private void Awake()
    {
        gunsParticles = GetComponentsInChildren<ParticleSystem>();
        droneSight = GetComponentInChildren<DroneSight>();

        droneSight.OnEnemyOnSight += enemy => enemyInRange = enemy;
        droneSight.OnEnemyLostSight += enemy => enemyInRange = null;
    }

    private void Update()
    {
        ProcessInput();
        CameraRotation();
        RayCast();
    }

    private void ProcessInput()
    {
        //Forward Dash
        if(Input.GetKeyDown(KeyCode.W) && dashFirstButtonPressed && canDash)
        {
             if(Time.time - dashTimeOfFirstButton < timeToDoublePress)
             {
                actualDashSpeedMultiply = dashMultiply;
                StartCoroutine(ResetSpeed());
             }
             dashReset = true;
        }
        if(Input.GetKey(KeyCode.W) && !dashFirstButtonPressed)
        {
            dashFirstButtonPressed = true;
            dashTimeOfFirstButton = Time.time;
        }
        if(dashReset)
        {
            dashFirstButtonPressed = false;
            dashReset = false;
        }

         //Movement
        transform.Translate(new Vector3 (Input.GetAxis(horizontalAxys),Input.GetAxis(verticalAxys),Input.GetAxis(forwardlAxys) * actualDashSpeedMultiply)* Time.deltaTime * movementSpeed);

        //Fire 1 (guns)
        if(Input.GetButton(fire1Button))
        {
            OnPrimaryWeaponFire?.Invoke();
            if(enemyInRay != null)
                enemyInRay.Damage();
        }
        if(Input.GetButtonDown(fire1Button))
        {
            foreach (var particle in gunsParticles)
            {
                particle.Play();
            }
        }
        if(Input.GetButtonUp(fire1Button))
        {
            foreach (var particle in gunsParticles)
            {
                particle.Stop();
            }
        }
        
        //Fire 2 (missiles)
        if(Input.GetButtonUp(fire2Button))
        {
            if(enemyInRange == null)
                return;
            OnSecondaryWeaponFire?.Invoke();

            for (int i = 0; i < missileSpawnLocation.Length; i++)
            {
                GameObject missileObj = Instantiate(missile, missileSpawnLocation[i].position,Quaternion.identity);
                missileObj.GetComponent<Missile>().target = enemyInRange.GameObject.transform;
            }
        }

        //Fire 3 (special)
        if(Input.GetButtonDown(specialButton))
        {
            Debug.Log("rewind started");
        }
        if(Input.GetButtonUp(specialButton))
        {
            Debug.Log("rewind finished");
        }
    }

    private void CameraRotation()
    {
        yawRotation += rotationSpeedHorizontal * Input.GetAxis(mouseXAxys);
        pitchRotation -= rotationSpeedVertical * Input.GetAxis(mouseYAxys);
        transform.eulerAngles = new Vector3(Mathf.Clamp(pitchRotation, -cameraPitchClamp, cameraPitchClamp), yawRotation, 0.0f);
    }

    private IEnumerator ResetSpeed()
    {
        canDash = false;
        yield return new WaitForSeconds(1f);
        actualDashSpeedMultiply = 1;
        canDash = true;
    }

    private void RayCast()
    {
        RaycastHit objectHit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * acquireEnemyDistance, Color.green);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out objectHit, acquireEnemyDistance))
        {
            if(objectHit.transform.GetComponent<IEnemy>() !=null)
            {
                enemyInRay = objectHit.transform.GetComponent<IEnemy>();
            }
            else
            {
                enemyInRay = null;
            }
        }
    }
}
