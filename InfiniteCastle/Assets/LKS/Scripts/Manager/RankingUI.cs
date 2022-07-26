using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingUI : MonoBehaviour
{
    private Text[] rankingScoreTexts;
    private Button toHome;
    private HomeSceneManager homeSceneManager;

    private void OnEnable()
    {
        homeSceneManager = GameObject.FindObjectOfType<HomeSceneManager>();
        rankingScoreTexts = transform.Find("Score").GetComponentsInChildren<Text>();
        toHome = transform.Find("ToHone").GetComponent<Button>();
        toHome.onClick.AddListener(GoHome);

        SetRank();
    }

    private void GoHome()
    {
        homeSceneManager.EndRanking();
    }

    private void SetRank()
    {
        for (int i = 0; i < rankingScoreTexts.Length; i++)
        {
            rankingScoreTexts[i].text = $"{PlayerPrefs.GetInt(RankingManager.Inst.KeyString[i])}";
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
}
