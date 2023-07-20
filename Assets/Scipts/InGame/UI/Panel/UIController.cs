using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance //singleton
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UIController>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("UIController");
                    instance = instanceContainer.AddComponent<UIController>();
                }
            }
            return instance;
        }
    }
    private static UIController instance;

    public GameObject JoyStickGO;
    public GameObject JoyStickPanelGO;
    public GameObject SlotMachineGO;
    public GameObject RouletteGO;
    public GameObject EndGameGO;
    public GameObject BonusPanelGO;

    public Text ClearRoomCnt;

    public Slider PlayerExpBar;
    public Slider BossHpBar;
    public Slider BossBackHpSlider;
    public bool backHpHit = false;
    public bool bossRoom = false;

    public Text playerLvText;

    public float BossCurrentHp;
    public float BossMaxHp;

    private void Start()
    {
        PlayerExpBar.value = PlayerData.Instance.playerCurrentExp / PlayerData.Instance.playerLvUpExp;
        PlayerExpBar.gameObject.SetActive(true);
        BossHpBar.gameObject.SetActive(false);
        BossBackHpSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!bossRoom)
        {
            PlayerExpBar.value = Mathf.Lerp(PlayerExpBar.value, PlayerData.Instance.playerCurrentExp / PlayerData.Instance.playerLvUpExp, 0.25f);
            playerLvText.text = "Lv." + PlayerData.Instance.playerLv;
        }
        else
        {
            
            if (backHpHit)
            {
                BossHpBar.value = BossCurrentHp / BossMaxHp;

                BossBackHpSlider.value = Mathf.Lerp(BossBackHpSlider.value, BossHpBar.value, Time.deltaTime * 10f);
                if (BossHpBar.value >= BossBackHpSlider.value - 0.01f)
                {
                    backHpHit = false;
                    BossBackHpSlider.value = BossHpBar.value;
                }
            }            
        }
    }
    public void Dmg()
    {
        Invoke("BackHpFun", 0.1f);
    }
    void BackHpFun()
    {
        backHpHit = true;
    }

    public void CheckBossRoom(bool isBossRoom)
    {
        bossRoom = isBossRoom;

        if (isBossRoom)
       {
            PlayerExpBar.gameObject.SetActive(false);
            BossHpBar.gameObject.SetActive(true);
            BossBackHpSlider.gameObject.SetActive(true);
        }
        else
        {
            PlayerExpBar.gameObject.SetActive(true);
            BossHpBar.gameObject.SetActive(false);
            BossBackHpSlider.gameObject.SetActive(false);
        }
    }

    public void TurnOnBonusPanel()
    {
        ActiveJoyStick(false);
        BonusPanelGO.SetActive(true);
    }

    public void CloseBonusPanel(string bonusName)
    {
        ActiveJoyStick(true);
        BonusPanelGO.SetActive(false);
        StartCoroutine(PlayerData.Instance.UseReward(bonusName));
    }

    public void TurnOnSlotMachine(bool isSlotMachineOn)
    {
        if (isSlotMachineOn)
        {
            ActiveJoyStick(false);
            SlotMachineGO.SetActive(true);
        }
        else
        {
            ActiveJoyStick(true);
            SlotMachineGO.SetActive(false);
        }
    }



    public void TurnOnRouletteMachine()
    {
        ActiveJoyStick(false);
        RouletteGO.SetActive(true);      
    }

    public void CloseRouletteMachine(string rewardName)
    {
        ActiveJoyStick(true);
        RouletteGO.SetActive(false);
        StartCoroutine(PlayerData.Instance.UseReward(rewardName));       
    }

    public void EndGame()
    {
        ActiveJoyStick(false);
        StartCoroutine(EndGamePopUp());
    }

    private void ActiveJoyStick(bool isActive)
    {
        JoyStickGO.SetActive(isActive);
        JoyStickPanelGO.SetActive(isActive);
    }
    IEnumerator EndGamePopUp()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        EndGameGO.SetActive(true);
        ClearRoomCnt.text = "Clear" + (StageManager.Instance.currentStage - 1);
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
