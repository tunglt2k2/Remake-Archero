using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBolt : EnemyProjectile
{
    Rigidbody rb;
    private void Start()
    {
        Destroy(gameObject, 5f);
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 10f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
