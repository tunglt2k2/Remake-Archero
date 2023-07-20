using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polyarl_Golem : BossMonster
{
    public GameObject BossBolt;
    public GameObject meleeAttackArea;
    public Transform AttackPointCenter;
    public Transform AttackPointLeft;
    public Transform AttackPointRight;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay1000 = new WaitForSeconds(1f);
    private new void Start()
    {
        base.Start();
        attackCoolTime = 2f;
        attackCoolTimeCacl = attackCoolTime;

        playerRealizeRange = 13f;
        attackRange = 50f;
        moveSpeed = 1f;
        nvAgent.stoppingDistance = 0.5f;
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
        maxHp = maxHp += StageManager.Instance.currentStage * 100;
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

        int num1 = Random.Range(0, 2) * 2 - 1; //Random -1 or 1
        int num2 = Random.Range(0, 2) * 2 - 1; //Random -1 or 1
        nvAgent.SetDestination(transform.parent.position - Vector3.right * 3 * num1 - Vector3.forward* 3 * num2);
        nvAgent.isStopped = false;
        nvAgent.speed = 3f;
        yield return Delay1000;
        nvAgent.isStopped = true;
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
            case 2:
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
                {
                    anim.SetTrigger("GetHit");
                }
                break;
        }
        canAttack = false;
        currentState = State.Idle;
    }


    public void Attack01()
    {
        GenerateBolt(new Vector3(0, -25f, 0),AttackPointLeft, damage);
        GenerateBolt(new Vector3(0, 0f, 0), AttackPointLeft, damage);
        GenerateBolt(new Vector3(0, 25f, 0), AttackPointLeft, damage);
    }

    public void Attack02()
    {
        GenerateBolt(new Vector3(0, -25f, 0), AttackPointLeft, damage);
        GenerateBolt(new Vector3(0, 0f, 0), AttackPointLeft, damage);
        GenerateBolt(new Vector3(0, 25f, 0), AttackPointLeft, damage);
    }

    public void AttackCenter()
    {
        GenerateBoltCenter(new Vector3(0, -45f, 0), damage);
        GenerateBoltCenter(new Vector3(0, -90f, 0), damage);
        GenerateBoltCenter(new Vector3(0, -135f, 0), damage);
        GenerateBoltCenter(new Vector3(0, -180f, 0), damage);
        GenerateBoltCenter(new Vector3(0, 0f, 0), damage);
        GenerateBoltCenter(new Vector3(0, 45f, 0), damage);
        GenerateBoltCenter(new Vector3(0, 90f, 0), damage);
        GenerateBoltCenter(new Vector3(0, 135f, 0), damage);
    }

    private void GenerateBolt(Vector3 rotation, Transform attackpoint, float _damage)
    {
        GameObject boltClone = Instantiate(BossBolt, attackpoint.position, Quaternion.Euler(transform.eulerAngles + rotation));
        if (boltClone.GetComponent<BossBolt>() != null)
        {
            boltClone.GetComponent<BossBolt>().damage = _damage;
        }
        else
        {
            Debug.Log("Generate Bolt wrong because BossBolt null");
        }
    }

    private void GenerateBoltCenter(Vector3 rotation, float _damage)
    {
        GameObject boltClone = Instantiate(BossBolt, AttackPointCenter.position, Quaternion.Euler(transform.eulerAngles + rotation));
        boltClone.transform.position += boltClone.transform.forward * 3f;
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
