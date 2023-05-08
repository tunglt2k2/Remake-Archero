using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<StageManager>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("StageManager");
                    instance = instanceContainer.AddComponent<StageManager>();
                }
            }
            return instance;
        }
    }
    private static StageManager instance;

    private GameObject player;

    public GameObject OpenDoor;
    public GameObject CloseDoor;

    [System.Serializable]
    public class StartPositionArray
    {
        public List<Transform> StartPosition = new List<Transform>();
    }

    public StartPositionArray[] startPositionArrays;
    //startPositionArrays[0] green stage
    //startPositionArrays[1] blue stage

    public List<Transform> StartPositionAngel = new List<Transform>();
    public List<Transform> StartPositonBoss = new List<Transform>();
    public Transform StartPositonLastBoss;

    public int currentStage = 0;
    int LastStage = 20;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");       
    }

    public void NextStage()
    {
        currentStage++;
        if (currentStage > LastStage)
        {
            UIController.Instance.EndGame();
            return;
        }
        int randomIndex;
        if(currentStage % 5 != 0) //normal stage
        {
            int arrayIndex = currentStage / 10;
            //Random a room
            randomIndex = Random.Range(0, startPositionArrays[arrayIndex].StartPosition.Count);
            //Active the room after player go to
            startPositionArrays[arrayIndex].StartPosition[randomIndex].parent.gameObject.SetActive(true);
            //Move player to this room
            player.transform.position = startPositionArrays[arrayIndex].StartPosition[randomIndex].position;
            //Remove room at list
            startPositionArrays[arrayIndex].StartPosition.RemoveAt(randomIndex);
        }
        else //bossRoom or angel
        {
            if(currentStage %10 == 5) //Angel
            {
                randomIndex = Random.Range(0, StartPositionAngel.Count);
                StartPositionAngel[randomIndex].parent.gameObject.SetActive(true);
                player.transform.position = StartPositionAngel[randomIndex].position;
                StartPositionAngel.RemoveAt(randomIndex);
            }
            else //Boss
            {
                if (currentStage == LastStage)
                { //LastBoss
                    StartPositonLastBoss.parent.gameObject.SetActive(true);
                    player.transform.position = StartPositonLastBoss.position;
                }
                else //Midboss
                {
                    randomIndex = Random.Range(0, StartPositonBoss.Count);
                    StartPositonBoss[randomIndex].parent.gameObject.SetActive(true);
                    player.transform.position = StartPositonBoss[randomIndex].position;
                    StartPositonBoss.RemoveAt(randomIndex);
                }
                UIController.Instance.CheckBossRoom(true);
            }
        }
        CameraMovement.Instance.CameraNextRoom(); 
    }

}
