using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineMgr : MonoBehaviour
{
    public GameObject[] SlotSkillObject;
    public Button[] Slot;
    public List<Skill> SkillToRandom = new List<Skill>();
    [System.Serializable]
    public class DisplayItemSlot
    {
        public List<Image> SlotSprite = new List<Image>();
    }
    public DisplayItemSlot[] DisplayItemSlots;

    public Text[] resultText;
    public List<int> StartList = new List<int>();
    public List<int> ResultIndexList = new List<int>();
    int ItemCnt = 3;
    int[] answer = { 2, 3, 1 };

    private void OnEnable()
    {
        SkillToRandom.Clear();
        foreach(Skill sk in PlayerData.Instance.skillInGameList)
        {
            SkillToRandom.Add(sk);
        }
        StartList.Clear();
        ResultIndexList.Clear();

        for(int i =0; i < 3; i++)
        {
            resultText[i].text = "";
            SlotSkillObject[i].transform.localPosition = new Vector3(0, 300f, 0);
        }

        for (int i = 0; i < SkillToRandom.Count; i++)
        {
            StartList.Add(i);
        }

        for (int i = 0; i < Slot.Length; i++)
        {
            for (int j = 0; j < ItemCnt; j++)
            {
                Slot[i].interactable = false;

                int randomIndex = Random.Range(0, StartList.Count);
                if (i == 0 && j == 1 || i == 1 && j == 0 || i == 2 && j == 2)
                {
                    ResultIndexList.Add(StartList[randomIndex]);
                }
                DisplayItemSlots[i].SlotSprite[j].sprite = SkillToRandom[StartList[randomIndex]].imageSkill;

                if (j == 0)
                {
                    DisplayItemSlots[i].SlotSprite[ItemCnt].sprite = SkillToRandom[StartList[randomIndex]].imageSkill;
                }
                StartList.RemoveAt(randomIndex);
            }
        }

        for (int i = 0; i < Slot.Length; i++)
        {
            StartCoroutine(StartSlot(i));
        }
    }

    IEnumerator StartSlot(int SlotIndex)
    {
        for (int i = 0; i < (ItemCnt * (6 + SlotIndex * 4) + answer[SlotIndex]) * 2; i++)
        {
            SlotSkillObject[SlotIndex].transform.localPosition -= new Vector3(0, 50f, 0);
            if (SlotSkillObject[SlotIndex].transform.localPosition.y < 50f)
            {
                SlotSkillObject[SlotIndex].transform.localPosition += new Vector3(0, 300f, 0);
            }
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i < ItemCnt; i++)
        {
            Slot[i].interactable = true; 
        }
        resultText[SlotIndex].text = SkillToRandom[ResultIndexList[SlotIndex]].name;
    }

    public void ClickBtn(int index)
    {
        UIController.Instance.TurnOnSlotMachine(false);
        PlayerData.Instance.UpgradeSkill(SkillToRandom[ResultIndexList[index]].name);
    }
}