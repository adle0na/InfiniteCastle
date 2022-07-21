using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeSceneManager : MonoBehaviour
{
    public GameObject settings;
    public Button startButton;
    public Button rankingButton;
    public Button settingsButton;
    public Button quitGameButton;
    public Button endSettingButton;

    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStart);
        rankingButton.onClick.AddListener(OnRanking);
        settingsButton.onClick.AddListener(OnSettingUI);
    }

    private void OnStart()
    {
        SceneManager.LoadScene(1);
    }

    private void OnRanking()
    {
        
    }

    private void OnSettingUI()
    {
        settings.SetActive(true);
        quitGameButton.onClick.AddListener(QuitGame);
        endSettingButton.onClick.AddListener(EndSettings);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void EndSettings()
    {
        settings.SetActive(false);
    }
}
