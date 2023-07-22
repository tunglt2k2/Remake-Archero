using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedProjectile : EnemyProjectile
{
    public float angle = 20f;
    public float speed = 2f;
    public Vector3 targetPosition;
    Rigidbody rb;
    float gravity = 9.81f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Destroy(gameObject, 3f);
        InitVeclocity();
        
    }

    void InitVeclocity()
    {
        float g = gravity * speed;
        float disX = targetPosition.x - transform.position.x;
        float disY = targetPosition.y - transform.position.y;
        float disZ = targetPosition.z - transform.position.z;
        float disXZ = Mathf.Sqrt(disX * disX + disZ * disZ);

        float V0 = disXZ / Mathf.Cos(angle) * Mathf.Sqrt(g / (2 * (Mathf.Tan(angle) * disXZ - disY)));
        float Vy = V0 * Mathf.Sin(angle);
        float Vx = V0 * Mathf.Cos(angle) * disX / disXZ;
        float Vz = V0 * Mathf.Cos(angle) * disZ / disXZ;
        GetComponent<Rigidbody>().velocity = new Vector3(Vx, Vy, Vz);
    }

    private void FixedUpdate()
    {
        Vector3 curr_g = - gravity * speed * Vector3.up;
        rb.AddForce(curr_g, ForceMode.Acceleration);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall") || other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

}
