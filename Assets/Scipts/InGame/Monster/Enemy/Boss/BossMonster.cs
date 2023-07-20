using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : EnemyMeleeFSM
{
    void Update()
    {
        if (currentHp <= 0) //moster die
        {
            if(nvAgent != null ) nvAgent.isStopped = true;
            rb.gameObject.SetActive(false);
            PlayerTargeting.Instance.MonsterList.Remove(transform.parent.gameObject);
            PlayerTargeting.Instance.TargetIndex = -1;
            Vector3 CurrentPostion = new Vector3(transform.position.x, 2f, transform.position.z);

            for (int i = 0; i < StageManager.Instance.currentStage / 10 + 2 + Random.Range(0, 3); i++)
            {
                GameObject ExpClone = Instantiate(PlayerData.Instance.ItemExp, CurrentPostion, transform.rotation);
                ExpClone.transform.parent = gameObject.transform.parent.parent;
            }
            UIController.Instance.CheckBossRoom(false);
            Destroy(transform.parent.gameObject);
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Potato"))
        {
            float potatodmg = other.gameObject.GetComponent<PlayerWeapon>().dmg;
            UIController.Instance.Dmg();
            GameObject eff = Instantiate(EffectSet.Instance.DuckDmgEffect, other.transform.position, Quaternion.Euler(90, 0, 0));
            eff.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            GameObject dmgTextClone = Instantiate(EffectSet.Instance.MonsterDmgText, transform.position, Quaternion.identity);

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

            Destroy(other.gameObject);
        }
    }
}
