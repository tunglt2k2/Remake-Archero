using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBolt : EnemyProjectile
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.up * -12f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject, 0.1f);
        }
    }

}
