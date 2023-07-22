using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMole : EnemyNormal
{
    public GameObject meleeAttackArea;
    private new void Start()
    {
        currentState = State.Move;
        base.Start();
        attackCoolTime = 2f;
        nvAgent.stoppingDistance = 0f;
        StartCoroutine(ResetAttackArea());
    }

    IEnumerator ResetAttackArea()
    {
        while (true)
        {
            yield return null;
            if (!meleeAttackArea.activeInHierarchy)
            {
                yield return new WaitForSeconds(attackCoolTime);
                meleeAttackArea.SetActive(true);
            }
        }
    }

    
    protected override void InitMonster()
    {
        maxHp += (StageManager.Instance.currentStage + 1) * 100f;
        currentHp = maxHp;
        damage += (StageManager.Instance.currentStage + 1) * 10f;
        moveSpeed = 2 + (StageManager.Instance.currentStage + 1) * 0.1f;
    }

    protected override IEnumerator Move()
    {
        yield return null;
        if (distance > playerRealizeRange)
        {
            nvAgent.SetDestination(transform.parent.position - Vector3.forward * 5f);
        }
        else
        {
            nvAgent.SetDestination(Player.transform.position);
            nvAgent.speed = moveSpeed;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            StartCoroutine(StopMove());
        }
    }

    IEnumerator StopMove()
    {
        nvAgent.isStopped = true;
        yield return new WaitForSeconds(2f);
        nvAgent.isStopped = false;
    }
}
