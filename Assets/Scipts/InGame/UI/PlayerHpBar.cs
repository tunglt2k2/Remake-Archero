using UnityEngine;
using UnityEngine.UI;
public class PlayerHpBar : MonoBehaviour
{
    public static PlayerHpBar Instance //singlton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerHpBar>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerHpBar");
                    instance = instanceContainer.AddComponent<PlayerHpBar>();
                }
            }
            return instance;
        }
    }
    private static PlayerHpBar instance;

    public Transform player;
    public Slider hpBar;
    public Slider BackHpSlider;
    public GameObject HpLineFolder;
    private float unitHp;
    private bool backHpHit;

    public Text playerHpText;
    private int numDefaultLineForder = 5;
    private void Start()
    {
        unitHp = PlayerData.Instance.maxHp / numDefaultLineForder;
        backHpHit = false;
    }
    private void Update()
    {
        transform.position = player.position;
        hpBar.value = Mathf.Lerp(hpBar.value, PlayerData.Instance.currentHp / PlayerData.Instance.maxHp, Time.deltaTime * 5f);

        if (backHpHit)
        {
            BackHpSlider.value = Mathf.Lerp(BackHpSlider.value, hpBar.value, Time.deltaTime * 10f);
            if (hpBar.value >= BackHpSlider.value - 0.01f)
            {
                backHpHit = false;
                BackHpSlider.value = hpBar.value;
            }
        }
        playerHpText.text = "" + PlayerData.Instance.currentHp;
    }

    public void Dmg()
    {
        Invoke("BackHpFun", 0.5f);
    }
    void BackHpFun()
    {
        backHpHit = true;
    }

    public void GetHpBoost()
    {
        PlayerData.Instance.maxHp += 1000;
        PlayerData.Instance.currentHp += 1000;
        float scaleX = numDefaultLineForder / (PlayerData.Instance.maxHp / unitHp);
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach( Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);

    }
}
