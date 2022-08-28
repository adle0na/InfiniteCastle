using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeSceneManager : MonoBehaviour
{
    public GameObject settings;
    public GameObject main;
    public GameObject ranking;
    
    public Button startButton;
    public Button rankingButton;
    public Button settingsButton;

    private void Start()
    {
        SoundManager.Inst.PlayBGM(BGMEnum.HomeScene);
        settings.SetActive(false);
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStart);
        rankingButton.onClick.AddListener(OnRanking);
        settingsButton.onClick.AddListener(OnSettingUI);
    }
    
    private void OnDisable()
    {
        startButton.onClick.RemoveAllListeners();
        rankingButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
    }

    private void OnStart()
    {
        SceneManager.LoadScene(1);
        SoundManager.Inst.PlayBGM(BGMEnum.PlayScene);
    }

    private void OnRanking()
    {
        SoundManager.Inst.PlaySFX(SFXEnum.Button);
        main.SetActive(false);
        ranking.SetActive(true);
    }

    public void EndRanking()
    {
        SoundManager.Inst.PlaySFX(SFXEnum.Button);
        ranking.SetActive(false);
        main.SetActive(true);
    }
    
    private void OnSettingUI()
    {
        SoundManager.Inst.PlaySFX(SFXEnum.Button);
        settings.SetActive(true);
    }
}
