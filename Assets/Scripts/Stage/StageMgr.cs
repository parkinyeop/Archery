using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMgr : MonoBehaviour
{
    static StageMgr instance;

    public static StageMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageMgr>();

                if (instance == null)
                {
                    var instanceContainer = new GameObject("StageMgr");
                    instance = instanceContainer.AddComponent<StageMgr>();
                }
            }
            return instance;
        }
    }

    public GameObject player;

    [System.Serializable]
    public class StartPositionArray
    {
        public List<Transform> StartPosition = new List<Transform>();
    }

    public StartPositionArray[] startPositionArrays;

    public List<Transform> StartPositionAngel = new List<Transform>();

    public List<Transform> StartPositionBoss = new List<Transform>();

    public Transform StartPositionLastBoss;

    public int currentStage = 0;
    int LastStage = 20;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void NextStage()
    {
        currentStage++;
        if (currentStage > LastStage)
        {
            return;
        }

        if (currentStage % 5 != 0) //Normal Stage
        {
            int arrayIndex = currentStage / 10;
            int randomIndex = Random.Range(0, startPositionArrays[arrayIndex].StartPosition.Count);
            player.transform.position = startPositionArrays[arrayIndex].StartPosition[randomIndex].position;
            startPositionArrays[arrayIndex].StartPosition.RemoveAt(randomIndex);
        }
        else
        {
            if(currentStage % 10 == 5)  //Angel
            {
                int randomIndex = Random.Range(0, StartPositionAngel.Count);
                player.transform.position = StartPositionAngel[randomIndex].position;
            }
            else
            {
                if(currentStage == LastStage)
                {
                    player.transform.position = StartPositionLastBoss.position;
                }
                else
                {
                    int randomIndex = Random.Range(0,StartPositionBoss.Count);
                    player.transform.position = StartPositionBoss[randomIndex].position;
                    StartPositionBoss.RemoveAt(randomIndex / 10);
                }
            }
        }
        CameraMovement.Instance.CameraNextRoom();
    }
}
