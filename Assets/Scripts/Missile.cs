using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float lifeTime = 8f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool enemyMissile = false;

    private Vector3 targetDirectionStart;

    public Transform target;

    //public bool timeReversing = false;

    
    //private Stack<Vector3> positions = new Stack<Vector3>();

    IEnumerator Start()
    {
        targetDirectionStart = target.position - transform.position;
        transform.LookAt(target);
        yield return new WaitForSeconds(lifeTime);
        Explode();
    }

    void Update()
    {
        if(target != null)
        {
            if(!enemyMissile)
            {
                Vector3 direction = target.position - transform.position;
                transform.Translate(direction.normalized * (speed + Random.Range(-5, +5)) * Time.deltaTime, Space.World);
                transform.LookAt(target);
            }
            else
            {
                transform.Translate(targetDirectionStart.normalized * (speed + Random.Range(-5, +5)) * Time.deltaTime, Space.World);
            }
        }
        
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
        if(!enemyMissile)
        {
            IEnemy target = other.GetComponent<IEnemy>();
            if(target !=null)
            {
                Debug.Log("missile target enemy");
                other.GetComponent<IEnemy>().Damage();
                Explode();
            }
        }
    }

    private void Explode()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        explosion.Play();
        Destroy(gameObject,1f);
    }
}
