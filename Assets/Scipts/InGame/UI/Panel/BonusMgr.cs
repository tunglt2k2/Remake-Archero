using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusMgr : MonoBehaviour
{
    public SkillList BonusList;
    public Image OtherSkill;
    public Text OtherSkillText;

    int num;
    private void OnEnable()
    {
        num = Random.Range(0, BonusList.skills.Count);
        OtherSkill.sprite = BonusList.skills[num].imageSkill;
        OtherSkillText.text = BonusList.skills[num].name;

    }

    public void ClickHealSkill()
    {
        UIController.Instance.CloseBonusPanel("Heal");
    }

    public void ClickOtherSkill()
    {
        UIController.Instance.CloseBonusPanel(OtherSkillText.text);
    }
}
