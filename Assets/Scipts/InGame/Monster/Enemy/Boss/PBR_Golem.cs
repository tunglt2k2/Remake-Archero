using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBR_Golem : BossMonster
{
    public GameObject BossBolt;
    public GameObject meleeAttackArea;
    public Transform AttackPoint;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay1000 = new WaitForSeconds(1f);
    private new void Start()
    {
        base.Start();
        playerRealizeRange = 13f;
        attackRange = 50f;
        moveSpeed = 1f;
        StartCoroutine(ResetAttackArea());
    }

    IEnumerator ResetAttackArea()
    {
        while (true)
        {
            yield return null;
            if (!meleeAttackArea.activeInHierarchy && currentState == State.Attack)
            {
                yield return new WaitForSeconds(attackCoolTime);
                meleeAttackArea.SetActive(true);
            }
        }
    }

    protected override void InitMonster()
    {
        maxHp = maxHp += StageManager.Instance.currentStage* 100;
        currentHp = maxHp;
        damage = damage += StageManager.Instance.currentStage * 10;
        attackCoolTime = 2f;
    }

    protected override IEnumerator Move()
    {
        yield return null;
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            anim.SetTrigger("Walk");
        }       
        nvAgent.SetDestination(transform.parent.position - Vector3.forward * 3f);
        yield return Delay500;
        nvAgent.speed = 2f;
        nvAgent.stoppingDistance = 2f;
        nvAgent.isStopped = false;
        yield return Delay1000;

        currentState = State.Idle;
    }

    protected override IEnumerator Attack()
    {
        yield return null;
        int RandomAction = Random.Range(0, 3);
        nvAgent.isStopped = true;
        transform.LookAt(Player.transform.position);
        switch (RandomAction)
        {
            case 0:
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
                {
                    anim.SetTrigger("Attack01");
                }
                break;
            case 1:
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
                {
                    anim.SetTrigger("Attack02");
                }
                break;
            default:
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
                {
                    anim.SetTrigger("GetHit");
                }
                nvAgent.stoppingDistance = 1f;
                nvAgent.SetDestination(Player.transform.position);

                yield return Delay500;
                nvAgent.isStopped = false;
                nvAgent.speed = 200f;
                yield return Delay1000;
                break;
        }
        canAttack = false;
        currentState = State.Idle;
    }


    public void Attack01()
    {
        GenerateBolt(new Vector3(0, -35f, 0), damage);
        GenerateBolt(new Vector3(0, 0f, 0), damage);
        GenerateBolt(new Vector3(0, 35f, 0), damage);
    }

    public void Attack02()
    {
        GenerateBolt(new Vector3(0, -25f, 0), damage);
        GenerateBolt(new Vector3(0, -15f, 0), damage);
        GenerateBolt(new Vector3(0, -0f, 0), damage);
        GenerateBolt(new Vector3(0, 15f, 0), damage);
        GenerateBolt(new Vector3(0, 25f, 0), damage);
    }

    private void GenerateBolt(Vector3 rotation, float _damage )
    {
        GameObject boltClone = Instantiate(BossBolt, AttackPoint.position, Quaternion.Euler(transform.eulerAngles + rotation));
        if (boltClone.GetComponent<BossBolt>() != null)
        {
            boltClone.GetComponent<BossBolt>().damage = _damage;
        }
        else
        {
            Debug.Log("Generate Bolt wrong because BossBolt null");
        }
    }


}

