using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathFX;
    [SerializeField] Transform parent;
    [SerializeField] int scorePerHit = 10;
    [SerializeField] int hits = 10;

    //ScoreBoard scoreBoard; 

    void Start()
    {
        AddNonTriggerBoxCollider();
        //scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    private void AddNonTriggerBoxCollider()
    {
        BoxCollider boxcollider = gameObject.AddComponent<BoxCollider>();
        boxcollider.isTrigger = false; 
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHits();
        if (hits <= 0)
        {
            KillEnemy();
        }
    }

    private void ProcessHits()
    {
        //scoreBoard.ScoreHit(scorePerHit);
        hits--;
    }

    private void KillEnemy()
    {
        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        //fx.transform.parent = parent;
        Destroy(gameObject);
    }
}
