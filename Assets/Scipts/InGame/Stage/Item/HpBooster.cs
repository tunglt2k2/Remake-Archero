using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBooster : MonoBehaviour
{
    GameObject Player;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(WaitClearRoom());
    }

    IEnumerator WaitClearRoom()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {    
            transform.position = Vector3.Lerp(transform.position, Player.transform.position, 0.3f);
            yield return null;
        }
    }
}
