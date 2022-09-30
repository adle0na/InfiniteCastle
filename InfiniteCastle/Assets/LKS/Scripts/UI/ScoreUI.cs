using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
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
        
        homeButton.onClick.AddListener(OnHomeButton);
        retryButton.onClick.AddListener(OnRetryButton);
        
        if (!gameManager.Player.IsAlive)
        {
            RefreshScores();
        }
    }

    private void OnDisable()
    {
        homeButton.onClick.RemoveAllListeners();
        retryButton.onClick.RemoveAllListeners();
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

        RankingManager.Inst.NewRank(total);
    }

    private void OnRetryButton()
    {
        SceneManager.LoadScene(1);
        gameManager.OnRestart();
    }

    private void OnHomeButton()
    {
        gameManager.OnRestart();
        SceneManager.LoadScene(0);
    }
}
