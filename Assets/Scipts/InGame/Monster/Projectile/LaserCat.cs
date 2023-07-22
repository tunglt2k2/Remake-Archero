using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCat : EnemyProjectile
{
    public GameObject HitEffect;
    public GameObject Laser;

    LineRenderer lr;

    void Start()
    {
        lr = Laser.GetComponent<LineRenderer>();
        lr.enabled = false;
        HitEffect.SetActive(false);

    }

    void Update()
    {
        if (lr.enabled)
        {
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hit);        
            if(hit.transform.CompareTag("Player") || hit.transform.CompareTag("Wall"))
            {
                HitEffect.SetActive(true);
                HitEffect.transform.position = hit.point;
                if (hit.transform.CompareTag("Player"))
                {
                    PlayerData.Instance.currentHp -= damage*Time.deltaTime * 5f;
                }
                
            }
            else
            {
                HitEffect.SetActive(false);
            }
        }    
    }
}
