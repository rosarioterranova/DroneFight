using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion;
    [SerializeField] float lifeTime = 8f;

    public Transform target;
    //public bool timeReversing = false;

    private float speed = 10f;
    //private Stack<Vector3> positions = new Stack<Vector3>();

    IEnumerator Start()
    {
        yield return new WaitForSeconds(lifeTime);
        Explode();
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * (speed + Random.Range(-5, +5)) * Time.deltaTime, Space.World);
        transform.LookAt(target);

        // if (!timeReversing)
        // {
        //     Vector3 dir = target.position - transform.position;
        //     transform.Translate(dir.normalized * (speed + Random.Range(-5, +5)) * Time.deltaTime, Space.World);
        //     transform.LookAt(target);
        //     positions.Push(transform.position);
        // }
        // else
        // {
        //     transform.position = positions.Pop();
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        IEnemy target = other.GetComponent<IEnemy>();
        if(target !=null & target == other.GetComponent<IEnemy>())
        {
            other.GetComponent<IEnemy>().Damage();
            Explode();
        }
    }

    private void Explode()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        explosion.Play();
        Destroy(gameObject,1f);
    }
}
