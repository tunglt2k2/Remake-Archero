using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteMgr : MonoBehaviour
{
    public GameObject RoulettePlate;
    public GameObject RouleteePanel;
    public Transform Needle;
    public SkillList RouletteReward;
    public Image[] DisplayItemSlot;
    private string[] nameReward = new string[7];

    List<int> StartList = new List<int>();
    int ItemCnt = 6;
    string nameResult;
    void OnEnable()
    {
        StartList.Clear();
        for (int i = 0; i <= ItemCnt; i++)
        {
            DisplayItemSlot[i].sprite = null;
            nameReward[i] = "";
        }

        for (int i = 0; i < ItemCnt; i++)
        {
            StartList.Add(i);
        }

        for (int i = 0; i < ItemCnt; i++)
        {
            int randomIndex = Random.Range(0, StartList.Count);
            DisplayItemSlot[i].sprite = RouletteReward.skills[StartList[randomIndex]].imageSkill;
            nameReward[i] = RouletteReward.skills[StartList[randomIndex]].name;
            StartList.RemoveAt(randomIndex);
        }

        StartCoroutine( StartRoulette() );
    }

    IEnumerator StartRoulette()
    {
        yield return new WaitForSeconds(2f);
        float rotateSpeed = 100f * Random.Range(1f, 5f);

        while (true)
        {
            yield return null;
            if (rotateSpeed <= 0.01f) break;
            rotateSpeed = Mathf.Lerp(rotateSpeed, 0, Time.deltaTime * 2f);
            RoulettePlate.transform.Rotate(0, 0, rotateSpeed);
        }

        yield return new WaitForSeconds(1f);
        Result();

        yield return new WaitForSeconds(1f);

        UIController.Instance.CloseRouletteMachine(nameResult);
    }

    private void Result()
    {
        int closetIndex = -1;
        float closetDis = 500f;
        float currentDis = 0f;

        for (int i = 0; i < ItemCnt; i++)
        {
            currentDis = Vector2.Distance(DisplayItemSlot[i].transform.position, Needle.position);

            if(closetDis > currentDis)
            {
                closetDis = currentDis;
                closetIndex = i;
            }
        }

        DisplayItemSlot[ItemCnt].sprite = DisplayItemSlot[closetIndex].sprite;
        nameResult = nameReward[closetIndex];
    }

    

}
