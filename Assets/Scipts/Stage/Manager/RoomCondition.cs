using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCondition : MonoBehaviour
{
    public List<GameObject> MonsterListInRoom = new List<GameObject>();
    public bool playerInThisRoom = false;
    public bool isClearRoom;

    GameObject NextGate;
    protected virtual void Start()
    {
        isClearRoom = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("NextRoom")){
                NextGate = transform.GetChild(i).gameObject;
            }
        }
    }

    protected virtual void Update()
    {
        if( playerInThisRoom)
        {
            if (PlayerTargeting.Instance.MonsterList.Count <= 0 && !isClearRoom)
            {
                isClearRoom = true;
                Debug.Log("Clear");
                StageManager.Instance.OpenDoor.SetActive(true);
                StageManager.Instance.CloseDoor.SetActive(false);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInThisRoom = true;
            PlayerTargeting.Instance.MonsterList = MonsterListInRoom;

            StageManager.Instance.CloseDoor.transform.position = NextGate.transform.position ;
            StageManager.Instance.OpenDoor.transform.position = NextGate.transform.position ;
            StageManager.Instance.OpenDoor.SetActive(false);
            StageManager.Instance.CloseDoor.SetActive(true);
        }
        else if (other.CompareTag("Monster"))
        {
            MonsterListInRoom.Add(other.transform.parent.gameObject);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (isClearRoom && playerInThisRoom && other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

}
