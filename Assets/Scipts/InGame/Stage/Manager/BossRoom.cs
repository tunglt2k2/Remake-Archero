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

    protected override IEnumerator CheckRoomEmpty()
    {
        while (true)
        {
            yield return null;
            if (playerInThisRoom)
            {
                if (PlayerTargeting.Instance.MonsterList.Count <= 0)
                {
                    yield return new WaitForSeconds(.5f);
                    if (PlayerTargeting.Instance.MonsterList.Count <= 0)
                    {
                        isClearRoom = true;
                        StageManager.Instance.OpenDoor.SetActive(true);
                        StageManager.Instance.CloseDoor.SetActive(false);
                        SpinGo.SetActive(true);
                        yield break;
                    }
                }
            }
        }
    }
}
