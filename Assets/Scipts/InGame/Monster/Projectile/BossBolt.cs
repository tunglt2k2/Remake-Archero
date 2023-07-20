using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBolt : MonoBehaviour
{
    Rigidbody rb;
    public float damage ;
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
        else if (collision.transform.CompareTag("Player"))
        {
            PlayerData.Instance.currentHp -= damage;
            PlayerHpBar.Instance.Dmg();
            PlayerMovement.Instance.TakenDamageAnim();
            Destroy(gameObject, 0.1f);
        }
    }
}
