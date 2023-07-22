using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySheep: EnemyNormal
{
    GameObject player;

    public LayerMask layerMaskDangerLine;

    public GameObject DangerMaker;
    public GameObject EnemyBolt;

    public Transform BoltGenPosition;

    new void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void InitMonster()
    {
        maxHp += (StageManager.Instance.currentStage + 1) * 200f;
        currentHp = maxHp;
        damage += (StageManager.Instance.currentStage + 1) * 20f;
    }

    IEnumerator WaitPlayer()
    {
        transform.LookAt(player.transform.position);
        DangerMakerShoot();

        yield return new WaitForSeconds(4f);
        Shoot();
    }
   
    void DangerMakerShoot()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Physics.Raycast(newPosition, transform.forward, out RaycastHit hit, 30f, layerMaskDangerLine);

        Debug.Log(hit.transform.tag);
        if (hit.transform.CompareTag("Wall"))
        {
            GameObject DangerMakerClone = Instantiate(DangerMaker, newPosition, transform.rotation);
            DangerMakerClone.GetComponent<DangerLine>().EndPosition = hit.point;
        }
    }

    void Shoot()
    {
        Vector3 currentRotation = transform.eulerAngles + new Vector3(-90, 0, 0);
        GameObject go = Instantiate(EnemyBolt, BoltGenPosition.position, Quaternion.Euler(currentRotation));
        go.GetComponent<EnemyProjectile>().damage = damage;
    }
    protected override IEnumerator Idle()
    {
        yield return new WaitForSeconds(3f);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            anim.SetTrigger("Idle");
        }
        StartCoroutine(WaitPlayer());
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
        if (Vector3.Distance(transform.position, Player.transform.position) < playerRealizeRange)
        {
            nvAgent.isStopped = false;
            nvAgent.SetDestination(transform.parent.position + randomVector);
            yield return new WaitForSeconds(2f);

        }
        nvAgent.isStopped = true;
        currentState = State.Idle;

    }
}
