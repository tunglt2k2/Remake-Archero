using UnityEngine;
using UnityEngine.UI;
public class EnemyHpBar : MonoBehaviour
{
    public Slider hpSlider;
    public Slider BackHpSlider;
    public bool backHpHit = false;

    public Transform enemy;
    private float maxHp ;
    private float currentHp;
    public Vector3 canvasOffset;
    private void Update()
    {
        transform.position = enemy.position + canvasOffset;
        maxHp = enemy.GetComponent<EnemyBase>().maxHp;
        currentHp = enemy.GetComponent<EnemyBase>().currentHp;
        hpSlider.value = Mathf.Lerp(hpSlider.value,currentHp / maxHp,Time.deltaTime * 5f);

        if (backHpHit)
        {
            BackHpSlider.value = Mathf.Lerp(BackHpSlider.value, hpSlider.value, Time.deltaTime * 10f);
            if(hpSlider.value >= BackHpSlider.value - 0.01f)
            {
                backHpHit = false;
                BackHpSlider.value = hpSlider.value;
            }
        }
    }

    public void Dmg()
    {
        Invoke("BackHpFun", 0.5f);
    }
    void BackHpFun()
    {
        backHpHit = true;
    }
}
