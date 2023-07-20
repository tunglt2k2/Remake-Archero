using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIController.Instance.TurnOnBonusPanel();
            Destroy(gameObject, 0.1f);
        }
    }
}
