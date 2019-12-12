using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] GameObject deathFX;
    //[SerializeField] Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
