using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin3CuBolt : EnemyProjectile
{
    new Rigidbody rigidbody;
    Vector3 newDir;
    int bounceCnt = 3;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        newDir = transform.up;
        rigidbody.velocity = newDir * -5f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            bounceCnt--;
            if(bounceCnt > 0)
            {
                newDir = Vector3.Reflect(newDir, collision.contacts[0].normal);
                rigidbody.velocity = newDir * -5f;
            }
            else
            {
                Destroy(gameObject, 0.1f);
            }
        }

        if (collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
