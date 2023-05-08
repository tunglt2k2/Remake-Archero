using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeFSM : EnemyBase  // Finite State Machine
{
    public enum State
    {
        Idle,
        Move,
        Attack,
    };

    public State currentState = State.Idle;

    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);
    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    protected new void Start()
    {
        base.Start();
        parentRoom = transform.parent.transform.parent.gameObject;
        Debug.Log("Start - State :" + currentState.ToString());
        StartCoroutine(FSM());
    }

    protected virtual void InitMonster() { }

    protected virtual IEnumerator FSM()
    {
        yield return null;

        while (!parentRoom.GetComponent<RoomCondition>().playerInThisRoom)
        {
            yield return Delay500;
        }

        InitMonster();

        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }  
    }

    protected virtual IEnumerator Idle()
    {
        yield return null;

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idle")){
            anim.SetTrigger("Idle");
        }

        if (CanAtkStateFun())
        {
            if (canAttack)
                currentState = State.Attack;
            else
            {
                currentState = State.Idle;
                transform.LookAt(Player.transform.position);
            }
        }
        else
            currentState = State.Move;
    }

    protected virtual void AttackEffect() { }

    protected virtual IEnumerator Attack()
    {
        yield return null;
        //Attack

        nvAgent.stoppingDistance= 0f;
        nvAgent.isStopped = true;
        nvAgent.SetDestination(Player.transform.position);

        yield return Delay500;

        nvAgent.isStopped = false;
        nvAgent.speed = 30f;
        canAttack = false;

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("stun"))
            anim.SetTrigger("Attack");

        AttackEffect();
        
        yield return Delay500;

        nvAgent.speed = moveSpeed;
        nvAgent.stoppingDistance = attackRange;
        currentState = State.Idle;
    }

    protected virtual IEnumerator Move()
    {
        yield return null;
        //Move
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            anim.SetTrigger("Walk");
        }
        if(CanAtkStateFun() && canAttack)
        {
            currentState = State.Attack;
        }
        else if (distance > playerRealizeRange)
        {
            nvAgent.SetDestination(transform.parent.position - Vector3.forward * 5f);
        }
        else
        {
            nvAgent.SetDestination(Player.transform.position);
        }
        
    }

}
