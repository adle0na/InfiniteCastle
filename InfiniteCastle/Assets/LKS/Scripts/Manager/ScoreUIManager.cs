using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        
        homeButton.onClick.AddListener(OnHomeButton);
        retryButton.onClick.AddListener(OnRetryButton);
        
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

        if (total > RankingManager.Inst.RankScores[RankingManager.Inst.RankScores.Length - 1])
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

    /// <summary>
    /// 구현 예정 방식
    /// Total score가 1위 숫자 이상일 시 숫자 옆에 New Best를 표시
    /// 점수를 PlayerPref에 저장 후 비교 알고리즘을 통해 5개를 선별
    /// 위에서부터 정렬하여 랭킹에 표시
    /// </summary>
    private void SaveScore()
    {
        
    }
}
