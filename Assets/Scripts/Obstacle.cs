using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] GameObject deathFX;
    //[SerializeField] Transform parent;

    private void OnParticleCollision(GameObject other)
    {
        DestroyObstacle();
    }

    private void DestroyObstacle()
    {
        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        //fx.transform.parent = parent;
        Destroy(gameObject);
    }
}
