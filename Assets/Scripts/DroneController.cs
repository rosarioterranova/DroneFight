using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{

    public Action OnPrimaryWeaponFire;
    public Action OnSecondaryWeaponFire;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeedHorizontal = 2.0f;
    [SerializeField] private float rotationSpeedVertical = 2.0f;
    [SerializeField] private GameObject missile;
    [SerializeField] private Transform[] missileSpawnLocation;

    private GameObject target;
    private float yawRotation = 0.0f;
    private float pitchRotation = 0.0f;
    private ParticleSystem[] gunsParticles;

    private void Awake()
    {
        gunsParticles = GetComponentsInChildren<ParticleSystem>();    
    }

    private void Update()
    {
        ProcessInput();
        Fly();
    }

    private void ProcessInput()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
        }
        if(Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.up * Time.deltaTime * movementSpeed);
        }
        if(Input.GetKey(KeyCode.E))
        {
            transform.Translate(Vector3.down * Time.deltaTime * movementSpeed);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            foreach (var particle in gunsParticles)
            {
                particle.Play();
            }
            
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            foreach (var particle in gunsParticles)
            {
                particle.Stop();
            }
        }
        if(Input.GetKey(KeyCode.Mouse0))
        {
            OnPrimaryWeaponFire?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("charged");
        }
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject b = Instantiate(missile, missileSpawnLocation[i].position,Quaternion.identity);
                b.GetComponent<Missile>().target = target.transform;
            }
            OnSecondaryWeaponFire?.Invoke();
        }
    }

    private void Fly()
    {
        yawRotation += rotationSpeedHorizontal * Input.GetAxis("Mouse X");
        pitchRotation -= rotationSpeedVertical * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(Mathf.Clamp(pitchRotation, -30, 30), yawRotation, 0.0f);
    }
}
