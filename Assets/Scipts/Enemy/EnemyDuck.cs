using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDuck : EnemyMeleeFSM
{
    public GameObject enemyCanvasGo;
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
    }

    protected override void AttackEffect()
    {
        Instantiate(EffectSet.Instance.DuckAttackEffect,transform.position,Quaternion.Euler(90,0,0));
    }
    void Update()
    {
        if(currentHp <= 0) //moster die
        {
            nvAgent.isStopped = true;
            rb.gameObject.SetActive(false);
            PlayerTargeting.Instance.MonsterList.Remove(transform.parent.gameObject);
            PlayerTargeting.Instance.TargetIndex = -1;
            Destroy(transform.parent.gameObject);
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Potato"))
        {
            enemyCanvasGo.GetComponent<EnemyHpBar>().Dmg();
            currentHp -= 250f;
            Instantiate(EffectSet.Instance.DuckDmgEffect, collision.contacts[0].point, Quaternion.Euler(90, 0, 0));
        }
    }
}
