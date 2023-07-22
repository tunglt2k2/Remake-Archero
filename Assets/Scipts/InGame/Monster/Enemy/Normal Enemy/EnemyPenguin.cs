using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPenguin : EnemyNormal
{
    RoomCondition roomConditionGO;
    GameObject player;
    LineRenderer lineRenderer;

    public LayerMask layerMaskDangerLine;
    public Transform BoltGenPosition;
    public GameObject EnemyBolt;

    private bool lookAtPlayer = true;
    new void Start()
    {
        base.Start();
        lineRenderer = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        roomConditionGO = transform.parent.transform.parent.gameObject.GetComponent<RoomCondition>();

        lineRenderer.startColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.endColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;

        StartCoroutine(WaitPlayer());
    }
    protected override void InitMonster()
    {
        maxHp += (StageManager.Instance.currentStage + 1) * 100f;
        currentHp = maxHp;
        damage += (StageManager.Instance.currentStage + 1) * 10f;
    }

    IEnumerator WaitPlayer()
    {
        lookAtPlayer = true;
        yield return null;
        while (!roomConditionGO.playerInThisRoom)
        {
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SetTarget());
        yield return new WaitForSeconds(2f);
        //Reset
        DangerMarkerDeactive();
        Shoot();
        //Delay for next attack
        yield return new WaitForSeconds(4f);
        StartCoroutine(WaitPlayer());
    }

    IEnumerator SetTarget()
    {
        while (true)
        {
            yield return null;
            if (!lookAtPlayer) break;
            transform.LookAt(player.transform.position);
            DangerMakerShoot();
        }
    }

    public void DangerMakerShoot()
    {
        Vector3 newPosition = BoltGenPosition.position;
        Vector3 newDir = transform.forward;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);

        for (int i = 1; i < 4; i++)
        {
            Physics.Raycast(newPosition, newDir, out RaycastHit hit, 30f, layerMaskDangerLine);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(i, hit.point);

            newPosition = hit.point;
            newDir = Vector3.Reflect(newDir, hit.normal);
        }
    }

    public void DangerMarkerDeactive()
    {
        lookAtPlayer = false;
        for(int i =0; i< lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, Vector3.zero);
        }
        lineRenderer.positionCount = 0;
    }
    public void Shoot()
    {
        Vector3 CurrentRotation = transform.eulerAngles + new Vector3(-90, 0, 0);
        GameObject go = Instantiate(EnemyBolt, BoltGenPosition.position, Quaternion.Euler(CurrentRotation));
        go.GetComponent<EnemyProjectile>().damage = damage;
    }

    protected override IEnumerator Idle()
    {
        yield return null;

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            anim.SetTrigger("Idle");
        }
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
        if (Vector3.Distance(transform.position, Player.transform.position) < playerRealizeRange )
        {
            nvAgent.isStopped = false;
            nvAgent.SetDestination(transform.parent.position + randomVector);
            yield return new WaitForSeconds(2f);

        }
        nvAgent.isStopped = true;

        yield return new WaitForSeconds(6f);
        currentState = State.Idle;

    }
}
