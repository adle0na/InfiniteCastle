using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIManager : MonoBehaviour
{
    private Text[] scoreTexts;
    private Button homeButton;
    private Button retryButton;
    
    public int floorScore = 10;
    public int monsterScore = 50;
    public int bossScore = 1000;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        scoreTexts = transform.Find("Scores").GetComponentsInChildren<Text>();
        homeButton = transform.Find("Home").GetComponent<Button>();
        retryButton = transform.Find("Retry").GetComponent<Button>();
        
        if (!gameManager.Player.IsAlive)
        {
            RefreshScores();
        }
    }

    private void RefreshScores()
    {
        int floor = gameManager.CurrentFloor * floorScore;
        scoreTexts[0].text = $"{floorScore} * {gameManager.CurrentFloor}\n{floor}";

        int monster = gameManager.KillMonsterCount * monsterScore;
        scoreTexts[1].text =
            $"{monsterScore} * {gameManager.KillMonsterCount}\n{monster}";

        int boss = gameManager.KillBossCount * bossScore;
        scoreTexts[2].text = $"{bossScore} * {gameManager.KillBossCount}\n{boss}";

        int total = floor + monster + boss;
        scoreTexts[3].text = $"{total}";
    }
}
