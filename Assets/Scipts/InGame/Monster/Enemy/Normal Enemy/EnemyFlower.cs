using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlower : EnemyNormal
{
    public GameObject projectilePrefab;
    public GameObject attackPoint;

    private new void Start()
    {
        base.Start();
        attackCoolTime = 5f;
        attackCoolTimeCacl = attackCoolTime;        
    }

    protected override void InitMonster()
    {
        maxHp += (StageManager.Instance.currentStage + 1) * 100f;
        currentHp = maxHp;
        damage += (StageManager.Instance.currentStage + 1) * 10f;
    }

    protected override void AttackEffect()
    {  }

    protected override IEnumerator Idle()
    {
        yield return null;
        if (canAttack) currentState = State.Attack;
    }

    protected override IEnumerator Attack()
    {
        yield return null;
        //Attack
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            anim.SetTrigger("Attack");

        AttackEffect();
        yield return null;
  
        canAttack = false;
        currentState = State.Idle;
    }

    private void Shoot()
    {
        InitProjectile(Player.transform.position);
        InitProjectile(Player.transform.position + new Vector3(1, 0, -1));
        InitProjectile(Player.transform.position + new Vector3(-1, 0, -1));
        InitProjectile(Player.transform.position + new Vector3(0, 0, 2));
    }

    private void InitProjectile(Vector3 position)
    {

        GameObject projectileInstance = Instantiate(projectilePrefab, attackPoint.transform.position, Quaternion.identity) as GameObject;
        projectileInstance.GetComponent<RedProjectile>().targetPosition = position;

    }
   
}
