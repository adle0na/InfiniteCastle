using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingUI : MonoBehaviour
{
    private Text[] rankingScoreTexts;
    private Button toHome;
    private Button reset;
    private HomeSceneManager homeSceneManager;

    private void OnEnable()
    {
        homeSceneManager = GameObject.FindObjectOfType<HomeSceneManager>();
        rankingScoreTexts = transform.Find("Score").GetComponentsInChildren<Text>();
        toHome = transform.Find("ToHone").GetComponent<Button>();
        toHome.onClick.AddListener(GoHome);

        SetRank();

        reset = transform.Find("Reset").GetComponent<Button>();
        reset.onClick.AddListener(ResetTest);
    }

    private void OnDisable()
    {
        toHome.onClick.RemoveAllListeners();
    }

    private void GoHome()
    {
        homeSceneManager.EndRanking();
    }

    private void SetRank()
    {
        for (int i = 0; i < rankingScoreTexts.Length; i++)
        {
            rankingScoreTexts[i].text = $"{RankingManager.Inst.RankScores[i]}";
        }
    }
    
    private void RankTest()
    {
        int testScore = 8593920;
        foreach (var testText in rankingScoreTexts)
        {
            testText.text = $"{testScore}";
            testScore -= 136031;
        }
    }

    private void ResetTest()
    {
        RankingManager.Inst.ResetRank();
        SetRank();
    }
}
