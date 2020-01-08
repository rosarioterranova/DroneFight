using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float rotationSpeedHorizontal = 2.0f;
    [SerializeField] float rotationSpeedVertical = 2.0f;

    float yawRotation = 0.0f;
    float pitchRotation = 0.0f;

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

        yawRotation += rotationSpeedHorizontal * Input.GetAxis("Mouse X");
        pitchRotation -= rotationSpeedVertical * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(Mathf.Clamp(pitchRotation, -20, 20), yawRotation, 0.0f);
        //transform.eulerAngles = new Vector3(pitchRotation, yawRotation, 0.0f);
    }
}
