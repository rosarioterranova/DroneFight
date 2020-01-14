using System;
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
    private Vector3[] positions;
    private int positionIndex = 0;

    public Transform target;
    public bool timeReversing = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        DroneController.OnRewindStart += () => timeReversing = true;
        DroneController.OnRewindFinish += () => { timeReversing = false; Array.Clear(positions,0,positions.Length);};
        positions = new Vector3[90];
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
        if (timeReversing)
        {
            transform.position = positions[positionIndex];
            if(positionIndex == 0) positionIndex = positions.Length -1;
            positionIndex--;
        }
        else
        {
            if(target != null)
            {
                if(!enemyMissile)
                {
                    Vector3 direction = target.position - transform.position;
                    transform.Translate(direction.normalized * (speed + UnityEngine.Random.Range(-5, +5)) * Time.deltaTime, Space.World);
                    transform.LookAt(target);
                }
                else
                {
                    transform.Translate(targetDirectionStart.normalized * (speed + UnityEngine.Random.Range(-5, +5)) * Time.deltaTime, Space.World);
                }
            }

            //Save positions for time reversing
            if(positionIndex == positions.Length) positionIndex = 0;
            positions[positionIndex] = transform.position;
            positionIndex++;
        }
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
