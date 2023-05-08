using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public static PlayerTargeting Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerTargeting>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerTargeting");
                    instance = instanceContainer.AddComponent<PlayerTargeting>();
                }
            }
            return instance;
        }
    }
    private static PlayerTargeting instance;

    public bool getATarget = false;
    
    float currentDistance = 0, closetDistance = 100f,TargetDistance = 100f;
    int closeDistIndex = 0, prevTargetIndex = -1;
    public int TargetIndex = -1;

    public LayerMask layerMask;

    public float attackSpeed = 1f;

    public List<GameObject> MonsterList = new List<GameObject>();

    public Transform AttackPoint;

    private void OnDrawGizmos()
    {
        if (getATarget)
        {
            for(int i =0; i< MonsterList.Count; i++)
            {
                if (MonsterList[i] == null) return;
                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position, out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster") )
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawRay(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position);

            }
        }
    }

    private void Update()
    {
        SetTarget();
        AttackTarget();
    }

    private void Attack()
    {
        PlayerMovement.Instance.anim.SetFloat("AttackSpeed",attackSpeed);
        Instantiate(PlayerData.Instance.PlayerBolt[PlayerData.Instance.PlayerSkill["Front Arrow"]], AttackPoint.position, transform.rotation);

        if(PlayerData.Instance.PlayerSkill["Diagonal Arrow"] > 0)
        {
            Instantiate(PlayerData.Instance.PlayerBolt[PlayerData.Instance.PlayerSkill["Diagonal Arrow"] - 1], AttackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, -45f, 0)));
            Instantiate(PlayerData.Instance.PlayerBolt[PlayerData.Instance.PlayerSkill["Diagonal Arrow"] - 1], AttackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, 45f, 0)));
        }

        if (PlayerData.Instance.PlayerSkill["Multishot"] > 0)
        {
            for (int i = 0; i < PlayerData.Instance.PlayerSkill["Multishot"]; i++)
            {
                Invoke("MultiShot", 0.2f * (i + 1));
            }
        }

    }

    void MultiShot()
    {
        Instantiate(PlayerData.Instance.PlayerBolt[PlayerData.Instance.PlayerSkill["Front Arrow"]], AttackPoint.position, transform.rotation);

        if (PlayerData.Instance.PlayerSkill["Diagonal Arrow"] > 0)
        {
            Instantiate(PlayerData.Instance.PlayerBolt[PlayerData.Instance.PlayerSkill["Diagonal Arrow"] - 1], AttackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, -45f, 0)));
            Instantiate(PlayerData.Instance.PlayerBolt[PlayerData.Instance.PlayerSkill["Diagonal Arrow"] - 1], AttackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, 45f, 0)));
        }
    }

    private void SetTarget()
    {
        if(MonsterList.Count != 0)
        {
            prevTargetIndex = TargetIndex;
            currentDistance = 0f;
            closeDistIndex = 0;
            TargetIndex = -1;

            for (int i = 0; i < MonsterList.Count; i++)
            {
                if (MonsterList[i] == null) return;
                currentDistance = Vector3.Distance(transform.position, MonsterList[i].transform.GetChild(0).position);

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position, out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    if (TargetDistance >= currentDistance)
                    {
                        TargetIndex = i;
                        TargetDistance = currentDistance;
                        if(!JoyStickMovement.Instance.isPlayerMoving && prevTargetIndex != TargetIndex)
                        {
                            TargetIndex = prevTargetIndex;
                        }
                    }
                }
                if (closetDistance >= currentDistance)
                {
                    closeDistIndex = i;
                    closetDistance = currentDistance;
                }
            }
            if(TargetIndex == -1)
            {
                TargetIndex = closeDistIndex;
            }
            closetDistance = 100f;
            TargetDistance = 100f;
            getATarget = true;
        }
    }

    private void AttackTarget() 
    { 
        if(TargetIndex == -1 || MonsterList.Count == 0)
        {
            PlayerMovement.Instance.anim.SetBool("Attack", false);
            return;
        }
        if (getATarget && !JoyStickMovement.Instance.isPlayerMoving && MonsterList.Count != 0)
        {
            //Debug.Log ( "lookat : " + MonsterList[TargetIndex].transform.GetChild (0));
            transform.LookAt(MonsterList[TargetIndex].transform.GetChild(0));

            if(UIController.Instance.bossRoom)
            {
                UIController.Instance.BossCurrentHp = MonsterList[TargetIndex].transform.GetChild(0).GetComponent<EnemyBase>().currentHp;
                UIController.Instance.BossMaxHp = MonsterList[TargetIndex].transform.GetChild(0).GetComponent<EnemyBase>().maxHp;
            }

            if (PlayerMovement.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                PlayerMovement.Instance.anim.SetBool("Walk", false);
                PlayerMovement.Instance.anim.SetBool("Idle", false);
                PlayerMovement.Instance.anim.SetBool("Attack", true);
            }
        }
        else if (JoyStickMovement.Instance.isPlayerMoving)
        {
            if (!PlayerMovement.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                PlayerMovement.Instance.anim.SetBool("Attack", false);
                PlayerMovement.Instance.anim.SetBool("Idle", false);
                PlayerMovement.Instance.anim.SetBool("Walk", true);
            }
        }
        else
        {
            PlayerMovement.Instance.anim.SetBool("Attack", false);
            PlayerMovement.Instance.anim.SetBool("Idle", true);
            PlayerMovement.Instance.anim.SetBool("Walk", false);
        }
    }


}
