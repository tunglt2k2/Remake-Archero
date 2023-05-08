using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour
{
    public float duration;
    public float damage = 100;
    private BoxCollider boxCollider;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerData.Instance.currentHp -= damage;
            PlayerHpBar.Instance.Dmg();
            PlayerMovement.Instance.TakenDamageAnim();
            StartCoroutine(DisableCollision(duration));
        }
    }

    private IEnumerator DisableCollision(float duration)
    {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(duration);
        boxCollider.enabled = true;
    }

}
