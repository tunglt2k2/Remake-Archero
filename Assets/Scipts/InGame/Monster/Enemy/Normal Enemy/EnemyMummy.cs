using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMummy : EnemyNormal
{
    public GameObject meleeAttackArea;
    public float timeAutoRotate = 2f;
    Vector3 NewDir;
    private new void Start()
    {
        currentState = State.Move;
        base.Start();
        attackCoolTime = 2f;
        StartCoroutine(ResetAttackArea());
        InitalVelocity();
        StartCoroutine(AutoRotate());
    }

    protected override void InitMonster()
    {
        maxHp += (StageManager.Instance.currentStage + 1) * 300f;
        currentHp = maxHp;
        damage += (StageManager.Instance.currentStage + 1) * 10f;
        moveSpeed = 3 + (StageManager.Instance.currentStage + 1) * 0.1f;
    }
    private void InitalVelocity()
    {
        transform.LookAt(Player.transform.position);
        NewDir = transform.forward;
        rb.velocity = NewDir * moveSpeed;
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

    IEnumerator AutoRotate()
    {
        float _time = timeAutoRotate;
        while (true)
        {
            yield return null;
            _time -= Time.deltaTime;
            if(_time <= 0)
            {
                _time = timeAutoRotate;
                InitalVelocity();
            }
        }
    }

    protected override IEnumerator Move()
    {
        yield return null;
        rb.velocity = NewDir * moveSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            NewDir = Vector3.Reflect(NewDir, collision.contacts[0].normal);
            rb.velocity = NewDir * moveSpeed;
            transform.forward = NewDir;
        }
    }

    
}
