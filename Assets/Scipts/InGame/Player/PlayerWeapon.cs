using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    int bounceCnt = 2;
    int wallBounceCnt = 2;   
    new Rigidbody rigidbody;
    public float dmg { get; private set; }
    Vector3 NewDir;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        NewDir = transform.forward;
        rigidbody.velocity = NewDir * 20f;
        dmg = PlayerData.Instance.dmg;
        Destroy(gameObject, 4f);
    }
    Vector3 ResultDir(int index)
    {
        int closetIndex = -1;
        float closetDis = 500f;
        float currentDis = 0f;

        for (int i = 0; i < PlayerTargeting.Instance.MonsterList.Count; i++)
        {
            if (i == index) continue;
            currentDis = Vector3.Distance(PlayerTargeting.Instance.MonsterList[i].transform.position, transform.position);

            if (currentDis > 5f) continue;

            if (closetDis > currentDis)
            {
                closetDis = currentDis;
                closetIndex = i;
            }
        }

        if (closetIndex == -1)
        {
            Destroy(gameObject, 0.2f);
            return Vector3.zero;
        }
        return (PlayerTargeting.Instance.MonsterList[closetIndex].transform.position - transform.position).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Monster"))
        {
            if(PlayerData.Instance.PlayerSkill["Ricochet"] != 0 && PlayerTargeting.Instance.MonsterList.Count >= 2)
            {
                int myIndex = PlayerTargeting.Instance.MonsterList.IndexOf(other.gameObject.transform.parent.gameObject);
                if(bounceCnt > 0)
                {
                    Debug.Log(bounceCnt);
                    bounceCnt--;
                    dmg *= 0.7f;
                    NewDir = ResultDir(myIndex) * 20f;
                    rigidbody.velocity = NewDir;
                    return;
                }
            }
            rigidbody.velocity = Vector3.zero;
            Destroy(gameObject);
        }
        else if (other.transform.CompareTag("Wall"))
        {
            if (PlayerData.Instance.PlayerSkill["Bouncy Wall"] == 0)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(gameObject);
            }
        }    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            if(PlayerData.Instance.PlayerSkill["Bouncy Wall"] != 0 && wallBounceCnt > 0)
            {
                wallBounceCnt--;
                dmg *= 0.5f;
                NewDir = Vector3.Reflect(NewDir, collision.contacts[0].normal);
                rigidbody.velocity = NewDir * 20f;
                return;
            }
            rigidbody.velocity = Vector3.zero;
            Destroy(gameObject, 0.1f);
        }        
    }
}
