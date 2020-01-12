using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
  public Transform target;   
  public float speed = 10f;
  public bool timeReversing = false;

  private  Stack<Vector3> positions = new Stack<Vector3>();

  void Update()
  {
    if(Input.GetKeyDown(KeyCode.Space))
    {
      timeReversing = true;
    }

    if(Input.GetKeyUp(KeyCode.Space))
    {
      timeReversing = false;
    }

    if(!timeReversing)
    {
      Vector3 dir = target.position - transform.position;
      transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
      transform.LookAt(target);
      positions.Push(transform.position);
    }
    else
    {
      transform.position = positions.Pop();
    }
  }
}
