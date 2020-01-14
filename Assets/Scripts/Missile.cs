using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float lifeTime = 8f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool enemyMissile = false;
    [SerializeField] private AudioClip start;
    [SerializeField] private AudioClip explode;

    private Vector3 targetDirectionStart;
    private AudioSource audioSource;
    private bool hit = false;

    public Transform target;

    //public bool timeReversing = false;

    
    //private Stack<Vector3> positions = new Stack<Vector3>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator Start()
    {
        audioSource.PlayOneShot(start,0.5f);
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
                other.GetComponent<IEnemy>().Damage();
                Explode();
                hit = true;
            }
        }
        else if(other.gameObject.layer == 10)
        {
            other.GetComponent<DroneController>().Damage();
            Explode();
        }
    }

    private void Explode()
    {
        audioSource.PlayOneShot(explode);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        explosion.Play();
        Destroy(gameObject,1f);
    }
}
