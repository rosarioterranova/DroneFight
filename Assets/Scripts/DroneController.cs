using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float rotationSpeedHorizontal = 2.0f;
    [SerializeField] float rotationSpeedVertical = 2.0f;
    [SerializeField] GameObject[] propellers;

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

        foreach (var propeller in propellers)
        {
            propeller.transform.RotateAround(propeller.transform.parent.transform.position, propeller.transform.parent.transform.up, Time.deltaTime * 1000f);
            //propeller.transform.localRotation = Quaternion.Euler(0, 45, 0);
            //propeller.transform.Rotate(Vector3.up *40f* Time.deltaTime);
        }

        yawRotation += rotationSpeedHorizontal * Input.GetAxis("Mouse X");
        pitchRotation -= rotationSpeedVertical * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(Mathf.Clamp(pitchRotation, -30, 30), yawRotation, 0.0f);
        //transform.eulerAngles = new Vector3(pitchRotation, yawRotation, 0.0f);
    }
}
