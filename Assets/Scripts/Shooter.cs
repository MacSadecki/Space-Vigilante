using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float basefiringRate = 0.2f;

    [Header("AI")]
    [SerializeField] bool useAI;
    [SerializeField] float firingRateVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;
    

    [HideInInspector] public bool isFiring;

    Coroutine firingCourotine;
    AudioPlayer audioPlayer;

    private void Awake() 
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }


    void Start()
    {
        if(useAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if (isFiring && firingCourotine == null)
        {
            firingCourotine = StartCoroutine(FireContinously());
        }
        else if(!isFiring && firingCourotine != null)
        {
            StopCoroutine(firingCourotine);
            firingCourotine = null;
        }
    }

    IEnumerator FireContinously()
    {
        while(true)
        {
            GameObject instance = Instantiate(projectilePrefab,
                                                transform.position, 
                                                Quaternion.identity);

            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            
            if(rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
            }
            
            Destroy(instance, projectileLifetime);

            float timeToNextProjectile = UnityEngine.Random.Range(basefiringRate - firingRateVariance, basefiringRate + firingRateVariance);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);

            audioPlayer.PlayShootingClip();

            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
}
