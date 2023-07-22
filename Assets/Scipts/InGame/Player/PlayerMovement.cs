using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance //singlton
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PlayerMovement>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("PlayerMovement");
                    instance = instanceContainer.AddComponent<PlayerMovement>();
                }
            }
            return instance;
        }
    }
    private static PlayerMovement instance;

    Rigidbody rb;
    public float moveSpeed = 5f;
   
    public Animator anim { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if(JoyStickMovement.Instance.joyVec.x != 0 || JoyStickMovement.Instance.joyVec.y != 0)
        {
            rb.velocity = new Vector3(JoyStickMovement.Instance.joyVec.x, 0, JoyStickMovement.Instance.joyVec.y) * moveSpeed;
            rb.rotation = Quaternion.LookRotation(new Vector3(JoyStickMovement.Instance.joyVec.x, 0, JoyStickMovement.Instance.joyVec.y));
        }
        gameObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("NextRoom"))
        {
            Debug.Log("Get next room");
            StageManager.Instance.NextStage();
        }

        if (other.transform.CompareTag("HpBooster"))
        {
            float _hpRecover = PlayerData.Instance.currentHp * 0.2f;
            PlayerData.Instance.RecoverCurentHp(_hpRecover);
            Destroy(other.transform.parent.gameObject);
        }

        if (other.transform.CompareTag("MeleeAtk"))
        {
            other.transform.gameObject.SetActive(false);
            PlayerData.Instance.currentHp -= other.transform.parent.GetComponent<EnemyBase>().damage;
            PlayerHpBar.Instance.Dmg();
            TakenDamageAnim();
        }

        if (other.transform.CompareTag("RangeAtk"))
        {
            Destroy(other.gameObject, 0.1f);
            PlayerData.Instance.currentHp -= other.transform.GetComponent<EnemyProjectile>()?.damage ?? 0;
            PlayerHpBar.Instance.Dmg();
            TakenDamageAnim();
        }

        if (PlayerTargeting.Instance.MonsterList.Count <= 0 && other.transform.CompareTag("EXP"))
        {
            PlayerData.Instance.PlayerExpCalc(100f);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("RangeAtk"))
        {
            Destroy(collision.gameObject, 0.1f);
            PlayerData.Instance.currentHp -= collision.transform.GetComponent<EnemyProjectile>()?.damage ?? 0;
            PlayerHpBar.Instance.Dmg();
            TakenDamageAnim();
        }
    }

    public void TakenDamageAnim()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
        {
            anim.SetTrigger("Dmg");
            Instantiate(EffectSet.Instance.PlayerDmgEffect, PlayerTargeting.Instance.AttackPoint.position, Quaternion.Euler(90, 0, 0));
        }
    }
}


