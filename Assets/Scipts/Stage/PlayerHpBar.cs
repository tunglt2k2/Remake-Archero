using UnityEngine;
using UnityEngine.UI;
public class PlayerHpBar : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Slider hpBar;
    public float maxHp;
    public float currentHp;

    private void Update()
    {
        transform.position = player.position;
        hpBar.value = currentHp / maxHp;
    }
}
