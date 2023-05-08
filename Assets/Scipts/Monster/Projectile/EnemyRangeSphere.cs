using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeSphere : MonoBehaviour
{
    public GameObject projectilePrefap;
    public GameObject attackPoint;
    public GameObject effectAttack;
    public float attackDuration = 2f ;
    private float attackColdDownTime;

    void Start()
    {
        attackColdDownTime = attackDuration;
    }

    void Update()
    {
        attackColdDownTime -= Time.deltaTime;
        if(attackColdDownTime < 0)
        {
            GameObject go1 = Instantiate(projectilePrefap, attackPoint.transform.transform.position, Quaternion.identity);
            go1.GetComponent<Rigidbody>().velocity = new Vector3(0,1,-1) * 10f;
            attackColdDownTime = attackDuration;
        }
    }
}
