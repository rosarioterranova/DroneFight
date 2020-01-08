using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float distance = 5f;
    [SerializeField]  float height = 10f;
    [SerializeField]  float heightDamping = 2.0f;
    [SerializeField]  float rotationDamping = 3.0f;

    float wantedRotationAngle;
    float wantedHeight;
    float currentRotationAngle;
    float currentHeight;
    Quaternion currentRotation;

    void LateUpdate ()
    {
       if (target){
           // Calculate the current rotation angles
           wantedRotationAngle = target.eulerAngles.y;
           wantedHeight = target.position.y + height;
           currentRotationAngle = transform.eulerAngles.y;
           currentHeight = transform.position.y;

           // Damp the rotation around the y-axis
           currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

           // Damp the height
           currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

           // Convert the angle into a rotation
           currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

           // Set the position of the camera on the x-z plane to:
           // distance meters behind the target
           transform.position = target.position;
           transform.position -= currentRotation * Vector3.forward * distance;

           // Set the height of the camera
           transform.position = new Vector3 (transform.position.x, currentHeight, transform.position.z);
           
           // Always look at the target
            transform.LookAt (target);
       }

    }
}
