using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExp : MonoBehaviour
{
    GameObject Player;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(WaitClearRoom());
    }

    IEnumerator WaitClearRoom()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            while (transform.parent.gameObject.GetComponent<RoomCondition>().isClearRoom)
            {
                transform.position = Vector3.Lerp(transform.position, Player.transform.position, 0.3f);
                yield return null;
            }
        }
    }
}
