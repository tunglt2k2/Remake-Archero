using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormal : EnemyMeleeFSM
{
    public GameObject enemyCanvasGo;
    protected virtual void Update()
    {
        if (currentHp <= 0) //moster die
        {
            if (nvAgent != null) nvAgent.isStopped = true;
            rb.gameObject.SetActive(false);
            PlayerTargeting.Instance.MonsterList.Remove(gameObject);
            Debug.Log("Die");
            PlayerTargeting.Instance.TargetIndex = -1;

            //Recover if have blood thirst skill;
            if (PlayerData.Instance.PlayerSkill["Blood Thirst"] == 1)
            {
                float _hpRecover = PlayerData.Instance.currentHp * 0.02f;
                PlayerData.Instance.RecoverCurentHp(_hpRecover);
            }

            //Random drop hp booster
            if(Random.value <= PlayerData.Instance.dropHpRate - (1 - StageManager.Instance.currentStage))
            {
                Instantiate(PlayerData.Instance.HpBoost, this.transform.position, Quaternion.identity);
            }

            //Drop exp
            Vector3 CurrentPostion = new Vector3(transform.position.x, 2f, transform.position.z);
            for (int i = 0; i < StageManager.Instance.currentStage / 10 + 2 + Random.Range(0, 3); i++)
            {
                GameObject ExpClone = Instantiate(PlayerData.Instance.ItemExp, CurrentPostion, transform.rotation);
                ExpClone.transform.parent = gameObject.transform.parent.parent;
            }

            if(gameObject.GetComponent<MonsterSplit>() != null)
            {
                gameObject.GetComponent<MonsterSplit>().Split();
            }

            Destroy(transform.parent.gameObject);
            return;
        }
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Potato"))
        {
            if (Random.value <= PlayerData.Instance.oneHitRate)
            {
                currentHp = 0;
                GameObject dmgTextClone = Instantiate(EffectSet.Instance.MonsterDmgText, transform.position, Quaternion.identity);
                dmgTextClone.GetComponent<DamageText>().DisplayOneHit();
            }
            else
            {
                float potatodmg = other.gameObject.GetComponent<PlayerWeapon>().dmg;
                GameObject eff = Instantiate(EffectSet.Instance.DuckDmgEffect, other.transform.position, Quaternion.Euler(90, 0, 0));
                eff.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                GameObject dmgTextClone = Instantiate(EffectSet.Instance.MonsterDmgText, transform.position, Quaternion.identity);
                enemyCanvasGo.GetComponent<EnemyHpBar>().Dmg();
                if (Random.value < PlayerData.Instance.critChange)
                {
                    currentHp -= potatodmg * PlayerData.Instance.critDmg;
                    dmgTextClone.GetComponent<DamageText>().DisplayDamage(potatodmg * PlayerData.Instance.critDmg, true);
                }
                else
                {
                    currentHp -= potatodmg;
                    dmgTextClone.GetComponent<DamageText>().DisplayDamage(potatodmg, false);
                }
            }

        }        
    }
    

}
