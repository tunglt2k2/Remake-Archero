using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHidden : EnemyNormal
{
    public Transform[] destinations;
    public GameObject projectilePrefab;
    public GameObject attackPoint;
    public GameObject effectAppear;
    public GameObject dummy;
    public GameObject mouth;
    SphereCollider sp;
    private void Awake()
    {
        sp = GetComponent<SphereCollider>();
    }
    private new void Start()
    {
        base.Start();
        rb.GetComponent<Rigidbody>();
        attackCoolTime = 3f;
        attackCoolTimeCacl = attackCoolTime;
        effectAppear.SetActive(false);
    }

    protected override void InitMonster()
    {
        maxHp += (StageManager.Instance.currentStage + 1) * 100f;
        currentHp = maxHp;
        damage += (StageManager.Instance.currentStage + 1) * 10f;
    }

    protected override void AttackEffect()
    { }


    protected override IEnumerator Idle()
    {
        yield return null;
        effectAppear.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        ActiveMonster(true);
        yield return new WaitForSeconds(0.5f);
        currentState = State.Attack;
    }

    protected override IEnumerator Attack()
    {
        yield return null;
        //Attack
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            anim.SetTrigger("Attack");

        AttackEffect();
        yield return new WaitForSeconds(2f);
        ActiveMonster(false);
        currentState = State.Move;
    }

    protected override IEnumerator Move()
    {
        yield return null;

        if (canAttack)
        {
            //Find position
            transform.parent.position = destinations[Random.Range(0, destinations.Length)].position ;
            //warnming eff
            yield return new WaitForSeconds(2f);
            currentState = State.Idle;
        }
    }

    void ActiveMonster(bool isActive) {
        canAttack = isActive;
        effectAppear.SetActive(isActive);
        dummy.SetActive(isActive);
        mouth.SetActive(isActive);
        enemyCanvasGo.SetActive(isActive);
        sp.enabled = isActive;
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
