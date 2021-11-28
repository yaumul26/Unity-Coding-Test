using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int score;
    public List<Vector3> tempRewardPosition = new List<Vector3>();
    public int urutan;
    
   public bool finishGame = false;
    void Start()
    {
        FindObjectOfType<GameField>().InitGameField(64, 64);

        int blockCount = 128;

        while (blockCount > 0)
        {
            int rdX = Random.Range(0, 64);
            int rdY = Random.Range(0, 64);
            if (FindObjectOfType<GameField>().IsCellBlocked(rdX, rdY))
                continue;

            FindObjectOfType<GameField>().BlockCell(rdX, rdY);
            blockCount--;
        }

        int rewardCount = 16;
        while (rewardCount > 0)
        {
            int rdX = Random.Range(0, 64);
            int rdY = Random.Range(0, 64);

            if (FindObjectOfType<GameField>().IsCellBlocked(rdX, rdY))
                continue;

            #region function find All Reward
            AICharacter aiCharacter = FindObjectOfType<AICharacter>();
            GameObject rewardTemp = FindObjectOfType<GameField>().CreateReward(rdX, rdY);
            tempRewardPosition.Add(rewardTemp.transform.position);
            #endregion

            rewardCount--;
        }
        FindObjectOfType<GameField>().InitAICharacter(0, 0);
        score = 0;
    }

    void Update()
    {
        if(!finishGame) 
            NextStepReward();
    }

    void NextStepReward()
    {
        AICharacter aiCharacter = FindObjectOfType<AICharacter>();
        if(urutan < tempRewardPosition.Count)
        {
            Queue<Vector3> queuePath = FindObjectOfType<AstarPathFinding>().GetPath(aiCharacter.transform.position, tempRewardPosition[urutan]);
            aiCharacter.SetPath(queuePath);

            if (Vector3.Distance(aiCharacter.transform.position, tempRewardPosition[urutan]) <= 0.2f)
            {
                if (urutan < tempRewardPosition.Count)
                {
                    score++;
                    urutan++;
                }
            }
        }
        else if(urutan >= tempRewardPosition.Count)
        {
            FindObjectOfType<UIController>().DisplayWinText();
            finishGame = true;
        }
        FindObjectOfType<UIController>().SetScore(score);
    }
}
