using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager
{
    private static RankingManager instance;

    public static RankingManager Inst
    {
        get
        {
            if (instance == null)
            {
                instance = new RankingManager();
                instance.Initialize();
            }

            return instance;
        }
    }

    private int rankCount = 5;
    private int[] rankScores = new int[5];
    private string[] keyString = new string[5];

    public int[] RankScores => rankScores;
    public string[] KeyString => keyString;

    private void Initialize()
    {
        for (int i = 0; i < rankCount; i++)
        {
            keyString[i] = $"Rank{i + 1}";
        }
        
        for (int i = 0; i < rankScores.Length; i++)
        {
            if (PlayerPrefs.HasKey(keyString[i]))
                rankScores[i] = PlayerPrefs.GetInt(keyString[i]);
            else
            {
                rankScores[i] = 0;
                PlayerPrefs.SetInt(keyString[i], rankScores[i]);
            }
        }
    }

    /// <summary>
    /// 새 점수 갱신시 호출하여 랭킹을 변경하는 함수
    /// 이 안에서 자체적으로 점수 비교 후 재정렬
    /// </summary>
    /// <param name="newScore"> 새 점수가 5위보다 높으면 새 점수를 매개변수로 호출 </param>
    public void NewRank(int newScore)
    {
        rankScores[rankScores.Length - 1] = newScore;

        for (int i = rankScores.Length - 1; i > 0; i--)
        {
            if (rankScores[i] > rankScores[i - 1])
            {
                (rankScores[i], rankScores[i - 1]) = (rankScores[i - 1], rankScores[i]);
            }
        }

        SaveScore();
    }

    /// <summary>
    /// 랭킹이 변경될 경우 PlayerPrefs에 저장하기 위한 함수.
    /// </summary>
    private void SaveScore()
    {
        for (int i = 0; i < rankScores.Length; i++)
        {
            PlayerPrefs.SetInt(keyString[i], rankScores[i]);
        }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// 테스트용 함수. 실제 적용하지 말 것.
    /// </summary>
    public void ResetRank()
    {
        for (int i = 0; i < rankScores.Length; i++)
        {
            PlayerPrefs.SetInt(keyString[i], 0);
        }
    }
}