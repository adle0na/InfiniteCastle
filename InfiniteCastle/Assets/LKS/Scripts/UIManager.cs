using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image hpBar;
    public Image[] bombers;
    public Text hpText;
    public Text floorText;
    public Text bossPattern;
    public Button leftJump;
    public Button rightJump;
    public Button bombButton;
    public GameObject gameOver;
    public GameObject bossUI;

    private string left = $"←";
    private string right = $"→";

    private PlayerController mainPlayer;
    private GameManager gameManager;

    #region Unity Built_In

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        mainPlayer = GameObject.FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        if (mainPlayer != null)
        {
            mainPlayer.onHpChange += RefreshHP;
            RefreshHP();
            leftJump.onClick.AddListener(OnLeftJump);
            rightJump.onClick.AddListener(OnRightJump);
            bombButton.onClick.AddListener(OnBombAttack);
        }
    }

    private void OnEnable()
    {
        SetGameOver(false);

        if (mainPlayer != null)
        {
            mainPlayer.onHpChange += RefreshHP;
        }
    }
    
    private void OnDisable()
    {
        if (mainPlayer != null)
        {
            mainPlayer.onHpChange -= RefreshHP;
        }
    }

    #endregion
    
    #region UI Refresh Methods

    private void RefreshHP()
    {
        hpBar.fillAmount = (float) mainPlayer.Health / mainPlayer.MaxHealth;
        hpText.text = $"{mainPlayer.Health} / {mainPlayer.MaxHealth}";
    }

    public void RefreshFloor()
    {
        floorText.text = $"{gameManager.CurrentFloor}";
    }

    public void RefreshBombCount()
    {
        foreach (var bomb in bombers)
        {
            bomb.color = new Color(0, 0, 0, 0.3f);
        }
        
        for (int i = 0; i < mainPlayer.BombCount; i++)
        {
            bombers[i].color = Color.white;
        }
    }

    public void RefreshBossPattern(Queue<int> bossPatternQueue)
    {
        string pattern = $" ";
        foreach (var bossPattern in bossPatternQueue)
        {
            if (bossPattern == -1)
                pattern += left;
            else
                pattern += right;
        }

        bossPattern.text = $"{string.Join(" ", pattern)}";
    }
    
    private void OnLeftJump()
    {
        Vector2 leftVec = new Vector2(-1, 0);
        mainPlayer.OnJumpInput(leftVec);
    }

    private void OnRightJump()
    {
        Vector2 rightVec = new Vector2(1, 0);
        mainPlayer.OnJumpInput(rightVec);
    }

    private void OnBombAttack()
    {
        mainPlayer.OnAttack();
    }

    public void SetBossUI(bool isOn)
    {
        bossUI.SetActive(isOn);
    }

    public void SetGameOver(bool toHow)
    {
        gameOver.SetActive(toHow);
    }

    #endregion
}
