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

    public GameObject player;

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
        if (currentStage > LastStage) return;
        int randomIndex;
        if(currentStage % 5 != 0) //normal stage
        {
            int arrayIndex = currentStage / 10;
            randomIndex = Random.Range(0, startPositionArrays[arrayIndex].StartPosition.Count);
            player.transform.position = startPositionArrays[arrayIndex].StartPosition[randomIndex].position;
        }
        else //bossRoom or angel
        {
            if(currentStage %10 == 5) //Angel
            {
                randomIndex = Random.Range(0, StartPositionAngel.Count);
                player.transform.position = StartPositionAngel[randomIndex].position;
            }
            else //Boss
            {
                if(currentStage == LastStage)  //LastBoss
                    player.transform.position = StartPositonLastBoss.position;
                else //Midboss
                {
                    randomIndex = Random.Range(0, StartPositonBoss.Count);
                    player.transform.position = StartPositonBoss[randomIndex].position;
                    StartPositonBoss.RemoveAt(currentStage / 10);
                }
            }
        }
        CameraMovement.Instance.CameraNextRoom(); 
    }


}
