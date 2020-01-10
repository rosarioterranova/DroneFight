using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float rotationSpeedHorizontal = 2.0f;
    [SerializeField] float rotationSpeedVertical = 2.0f;
    [SerializeField] GameObject[] propellers;
    [SerializeField] GameObject missile;
    [SerializeField] Transform[] missileSpawnLocation;

    [SerializeField] GameObject target;

    float yawRotation = 0.0f;
    float pitchRotation = 0.0f;
    ParticleSystem[] mainWeaponParticles;

    private void Awake()
    {
        mainWeaponParticles = GetComponentsInChildren<ParticleSystem>();    
    }

    void Update()
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
            foreach (var particle in mainWeaponParticles)
            {
                particle.Play();
            }
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            foreach (var particle in mainWeaponParticles)
            {
                particle.Stop();
            }
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
        }
        foreach (var propeller in propellers)
        {
            propeller.transform.RotateAround(propeller.transform.parent.transform.position, propeller.transform.parent.transform.up, Time.deltaTime * 1000f);
        }

        yawRotation += rotationSpeedHorizontal * Input.GetAxis("Mouse X");
        pitchRotation -= rotationSpeedVertical * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(Mathf.Clamp(pitchRotation, -30, 30), yawRotation, 0.0f);
    }
}
