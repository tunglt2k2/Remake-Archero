using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 20f;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Name" + other.transform.name);
        if(other.transform.CompareTag("Wall") || other.transform.CompareTag("Monster"))
        {
            Debug.Log("Name" + other.transform.name);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Name" + collision.transform.name);
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Monster"))
        {
            Debug.Log("Name" + collision.transform.name);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(gameObject);
        }
    }
}
