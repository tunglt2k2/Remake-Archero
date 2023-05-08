using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerData>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerData");
                    instance = instanceContainer.AddComponent<PlayerData>();
                }
            }
            return instance;
        }
    }
    private static PlayerData instance;

    public GameObject Player;
    [Header("Player Attribute")]
    public float dmg = 500;
    public float maxHp = 1000;
    public float currentHp = 1000;
    public float critChange = 0.1f;
    public float critDmg = 2f;
    public float dropHpRate = 0.05f;
    public float oneHitRate= 0f;
    public bool playerDead;

    public float playerCurrentExp { get; private set; }
    public float playerLvUpExp = 200;
    public int playerLv;

    [Header("Player Prefab Object")]
    public GameObject[] PlayerBolt; //List type of PlayerBolt
    public GameObject ItemExp;
    public GameObject HpBoost;

    public SkillList skillList;
    public List<Skill> skillInGameList = new List<Skill>(); 

    public Dictionary<string, int> PlayerSkill = new Dictionary<string, int>();

    /*
    PlayerSkill include: Attack Boost, Bouncy Wall, Crit Master, Diagonal Arrow, Front Arrow,
    HP Boost, Ricochet, Multishot,Front Arrow , NASPD
    */
    private void Awake()
    {
        foreach ( Skill sk in skillList.skills)
        {
            skillInGameList.Add(sk);
        }
        foreach (var sk in skillInGameList)
        {
            PlayerSkill.Add(sk.name, 0);
        }
    }

    private void Start()
    {
        playerCurrentExp = 0;
        playerLv = 1;
    }

    private void Update()
    {
        if(!playerDead && currentHp <= 0)
        {
            currentHp = 0;
            playerDead = true;
            PlayerMovement.Instance.anim.SetTrigger("Dead");
            UIController.Instance.EndGame();
            return;
        }
    }

    public void PlayerExpCalc(float exp)
    {
        playerCurrentExp += exp;
        if(playerCurrentExp >= playerLvUpExp)
        {
            playerLv++;
            playerCurrentExp -= playerLvUpExp;
            playerLvUpExp *= 1.3f;
            StartCoroutine(PlayerLevelUp());
        }
    }

    IEnumerator PlayerLevelUp()
    {
        yield return null;
        EffectSet.Instance.PlayerLevelUpEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        UIController.Instance.TurnOnSlotMachine(true);
        yield return new WaitForSeconds(1.5f);
        EffectSet.Instance.PlayerLevelUpEffect.SetActive(false);
    }

    public void UpgradeSkill(string name)
    {
        PlayerSkill[name] ++;

        if(name.Equals("Bouncy Wall") || name.Equals("Ricochet") || name.Equals("Blood Thirst") || name.Equals("Headshot"))
        {
            if (name.Equals("Headshot"))
            {
                oneHitRate = 0.3f;
            }
            if(FindIndexObjectWithName(name) != -1)
            {
                skillInGameList.RemoveAt(FindIndexObjectWithName(name));
            }
            
        }
        else if(name.Equals("NASPD"))
        {
            PlayerTargeting.Instance.attackSpeed *= 1.3f ;
        }
        else if(name.Equals("Crit Master"))
        {
            critChange += 0.3f;
            critDmg += 0.3f;
        }
        else if(name.Equals("Strong Heart"))
        {
            dropHpRate *= 1.4f;
        }
        else if(name.Equals("Attack Boost"))
        {
            dmg *= 1.3f;
        }
        else if (name.Equals("HP Boost"))
        {
            IncreaseMaxHp(currentHp * 0.2f);
        }

    }

    int FindIndexObjectWithName(string name)
    {

        for(int i =0; i< skillInGameList.Count; i++)
        {
            if (skillInGameList[i].name.Equals(name)) return i; 
        }
        return -1;
    }

    public void RecoverCurentHp(float hp)
    {
        currentHp = Mathf.Clamp((int)(currentHp + hp), 0, maxHp);
        GameObject recoverTxtClone = Instantiate(EffectSet.Instance.PlayerRecoverText, Player.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        recoverTxtClone.GetComponent<RecoverText>().DisplayRecover(hp);
    }

    public void IncreaseMaxHp(float hp)
    {
        maxHp += hp;
        currentHp += hp;
        PlayerHpBar.Instance.GetHpBoost();
        GameObject recoverTxtClone = Instantiate(EffectSet.Instance.PlayerRecoverText, Player.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        recoverTxtClone.GetComponent<RecoverText>().DisplayRecover(hp);
    }

}
