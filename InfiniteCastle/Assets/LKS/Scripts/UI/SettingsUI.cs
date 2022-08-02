using System;
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

    private float bgmVolum = 1.0f;
    private float sfxVolum = 1.0f;
    private string bgmVolumString = "BGMVolum";
    private string sfxVolumString = "SFXVolum";

    private void Start()
    {
        if (PlayerPrefs.HasKey(bgmVolumString) && PlayerPrefs.HasKey(sfxVolumString))
            Initailize();
    }

    private void OnEnable()
    {
        bgmSlider = transform.Find("BGM").GetComponent<Slider>();
        sfxSlider = transform.Find("SFX").GetComponent<Slider>();
        quitSettings = transform.Find("QuitSettings").GetComponent<Button>();
        quitGame = transform.Find("QuitGame").GetComponent<Button>();

        bgmSlider.value = bgmVolum;
        sfxSlider.value = sfxVolum;
        quitSettings.onClick.AddListener(EndSettings);
        quitGame.onClick.AddListener(QuitGame);
        bgmSlider.onValueChanged.AddListener(SetBGMSound);
        sfxSlider.onValueChanged.AddListener(SetSFXSound);
    }

    private void OnDisable()
    {
        quitGame.onClick.RemoveAllListeners();
        quitSettings.onClick.RemoveAllListeners();
        bgmSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
    }

    private void Initailize()
    {
        bgmVolum = PlayerPrefs.GetFloat(bgmVolumString);
        sfxVolum = PlayerPrefs.GetFloat(sfxVolumString);
        
        SetBGMSound(bgmVolum);
        SetSFXSound(sfxVolum);
    }

    private void SetBGMSound(float newValue)
    {
        //SoundManager.Inst.bgmAudioSource.volume = newValue;
        bgmVolum = newValue;
        SoundManager.Inst.audioMixer.SetFloat("BGM", Mathf.Log10(bgmVolum) * 20);
        PlayerPrefs.SetFloat(bgmVolumString, bgmVolum);

        if (bgmVolum != PlayerPrefs.GetFloat(bgmVolumString))
            PlayerPrefs.SetFloat(bgmVolumString, bgmVolum);
    }

    private void SetSFXSound(float newValue)
    {
        //SoundManager.Inst.sfxAudioSource.volume = newValue;
        sfxVolum = newValue;
        SoundManager.Inst.audioMixer.SetFloat("SFX", Mathf.Log10(sfxVolum) * 20);
        PlayerPrefs.SetFloat(sfxVolumString, sfxVolum);

        if (newValue != PlayerPrefs.GetFloat(sfxVolumString))
            PlayerPrefs.SetFloat(sfxVolumString, sfxVolum);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void EndSettings()
    {
        PlayerPrefs.Save();
        GameObject obj = GameObject.Find("SettingsUI");
        obj.SetActive(false);
    }
}