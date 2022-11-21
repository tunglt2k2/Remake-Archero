using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBase : MonoBehaviour
{
    public float maxHp = 1000f;
    public float currentHp = 1000f;

    public float damage = 100f;

    protected float playerRealizeRange = 10f;
    protected float attackRange = 5f;
    protected float attackCoolTime = 5f;
    protected float attackCoolTimeCacl = 5f;
    protected bool canAttack = true;

    protected float moveSpeed = 2f;

    public GameObject Player;
    public NavMeshAgent nvAgent;
    protected float distance;

    protected GameObject parentRoom;

    protected Animator anim;
    protected Rigidbody rb;

    public LayerMask layerMask;

    protected void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player:" + Player);
        Debug.Log("Player.transform.position:" + Player.transform.position);

        nvAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        parentRoom = transform.parent.transform.parent.gameObject;

        StartCoroutine(CalcCoolTime());
    }

    protected bool CanAtkStateFun()
    {
        Vector3 targetDirecton = new Vector3(Player.transform.position.x - transform.position.x, 0f, Player.transform.position.z - transform.position.z);
        Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z), targetDirecton, out RaycastHit hit, 30f, layerMask);
        distance = Vector3.Distance(Player.transform.position, transform.position);

        if (hit.transform == null)
        {
            Debug.Log("hit.transform == null");
            return false;
        }
        if(hit.transform.CompareTag("Player") && distance <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual IEnumerator CalcCoolTime()
    {
        while (true)
        {
            yield return null;
            if(!canAttack)
            {
                attackCoolTimeCacl -= Time.deltaTime;
                if(attackCoolTimeCacl <= 0)
                {
                    attackCoolTimeCacl = attackCoolTime;
                    canAttack = true;
                }
            }
        }
    }



}
