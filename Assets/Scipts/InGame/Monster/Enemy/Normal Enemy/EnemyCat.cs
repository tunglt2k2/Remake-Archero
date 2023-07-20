using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCat : EnemyNormal
{

    private bool lookAtPlayer = true;
    public GameObject LaserEffect;

    float attackTime =4f;
    float attackTimeCalc = 4f;

    new void Start()
    {
        base.Start();
        playerRealizeRange = 15f;
        LaserEffect.SetActive(false);

    }

    protected override void InitMonster()
    {
        maxHp += (StageManager.Instance.currentStage + 1) * 100f;
        currentHp = maxHp;
        damage += (StageManager.Instance.currentStage + 1) * 10f;
    }

    protected override IEnumerator Idle()
    {
        yield return null;
        
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            anim.SetTrigger("Idle");
        }

        StartCoroutine(WaitPlayer());
        StartCoroutine(LaserOff());
        yield return new WaitForSeconds(6f);
        currentState = State.Move;
        
        
    }

    protected override IEnumerator Move()
    {
        yield return null;
        //Move
        
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            anim.SetTrigger("Walk");
        }
        Vector3 randomVector = new Vector3(Random.Range(-4, 4), 0f, Random.Range(-4, 4));
        if (Vector3.Distance(transform.position, Player.transform.position) < playerRealizeRange && !LaserEffect.activeInHierarchy)
        {
            nvAgent.isStopped = false;
            nvAgent.SetDestination(transform.parent.position + randomVector);
            yield return new WaitForSeconds(2f);
            
        }
        nvAgent.isStopped = true;

        yield return new WaitForSeconds(6f);
        currentState = State.Idle;

    }

    IEnumerator LaserOff()
    {
        while (true)
        {
            yield return null;
            if (LaserEffect.activeInHierarchy)
            {
                attackTimeCalc -= Time.deltaTime;
                if (attackTimeCalc <= 0)
                {
                    attackTimeCalc = attackTime;
                    LaserEffect.SetActive(false);
                    yield break;
                }
            }
        }
    }

    IEnumerator WaitPlayer()
    {
        yield return new WaitForSeconds(2f);

        StartCoroutine(SetTarget());

        yield return new WaitForSeconds(2f);
        Shoot();

        StartCoroutine(Targeting());
    }

    IEnumerator SetTarget()
    {
        lookAtPlayer = true;
        LaserEffect.SetActive(true);

        while (true)
        {
            yield return null;
            if (!lookAtPlayer) break; 

            transform.LookAt(Player.transform.position);
        }
    }

    IEnumerator Targeting()
    {
        while (true)
        {
            yield return null;
            if (!LaserEffect.activeInHierarchy)
            {
                break;
            }
            Quaternion targetRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
        }
    }

    public void Shoot()
    {
        lookAtPlayer = false;
    }

    

}
