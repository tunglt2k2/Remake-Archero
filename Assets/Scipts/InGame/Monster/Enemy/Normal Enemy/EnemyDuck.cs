using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDuck : EnemyNormal
{
    public GameObject meleeAttackArea;
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, playerRealizeRange);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //}

    private new void Start()
    {
        base.Start();
        attackCoolTime = 2f;
        attackCoolTimeCacl = attackCoolTime;

        attackRange = 3f;
        nvAgent.stoppingDistance = 1f;
        StartCoroutine(ResetAttackArea());
    }

    IEnumerator ResetAttackArea()
    {
        while (true)
        {
            yield return null;
            if(!meleeAttackArea.activeInHierarchy && currentState == State.Attack)
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
        moveSpeed = 3 + (StageManager.Instance.currentStage + 1) * 0.1f;
    }

    protected override void AttackEffect()
    {
        Vector3 pos = transform.position + new Vector3(0f, 1f,0f ) ;
        Instantiate(EffectSet.Instance.DuckAttackEffect,pos ,Quaternion.Euler(90,0,0));
    }
    
}
