using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    private Slider bgmSlider;
    private Slider sfxSlider;
    private Button quitSettings;
    private Button quitGame;

    private void OnEnable()
    {
        bgmSlider = transform.Find("Sound").GetComponent<Slider>();
        sfxSlider = transform.Find("SFX").GetComponent<Slider>();
        quitSettings = transform.Find("QuitSettings").GetComponent<Button>();
        quitGame = transform.Find("QuitGame").GetComponent<Button>();

        quitSettings.onClick.AddListener(EndSettings);
        quitGame.onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        quitGame.onClick.RemoveAllListeners();
        quitSettings.onClick.RemoveAllListeners();
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void EndSettings()
    {
        GameObject obj = GameObject.Find("SettingsUI");
        obj.SetActive(false);
    }
}
