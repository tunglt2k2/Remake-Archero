using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : RoomCondition
{
    [SerializeField] private GameObject SpinGo;
    protected override void Start()
    {
        base.Start();
        SpinGo.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (playerInThisRoom)
        {
            if (PlayerTargeting.Instance.MonsterList.Count <= 0 && !isClearRoom)
            {
                isClearRoom = true;
                StageManager.Instance.OpenDoor.SetActive(true);
                StageManager.Instance.CloseDoor.SetActive(false);
                SpinGo.SetActive(true);
            }
        }
    }
}
