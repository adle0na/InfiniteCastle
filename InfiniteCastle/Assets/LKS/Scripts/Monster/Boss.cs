using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour, IAttackable
{
    private int attack = 10;
    private int health;
    private int maxHealth = 100;
    private bool isAlive = true;
    private bool isPatternSet = false;
    private int minPatternCount = 2;
    private int maxPatternCount = 5;

    private Queue<int> pattern;
    private GameManager gameManager;
    private Animator animator;

    protected readonly int hashOnDamage = Animator.StringToHash("onDamage");
    protected readonly int hashOnAttack = Animator.StringToHash("onAttack");
    protected readonly int hashIsAlive = Animator.StringToHash("isAlive");

    public bool IsAlive
    {
        get => isAlive;
        set
        { 
            isAlive = value;
            animator.SetBool(hashIsAlive, isAlive);
        }
    }

    public bool IsPatternSet
    {
        get => isPatternSet;
        set => isPatternSet = value;
    }

    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            if (health == 0)
                OnDie();
            else
                SetPattern();
            onHpChange?.Invoke();
        }
    }

    public int MaxHealth
    {
        get => maxHealth;
    }

    public int Attack
    {
        get => attack;
        set
        {
            attack = value;
            animator.SetTrigger(hashOnAttack);
        }
    }

    public HpDelegate onHpChange { get; set; }

    public Queue<int> Pattern => pattern;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        IsAlive = true;
        Health = MaxHealth;
        if (gameManager.KillBossCount > 0)
        {
            minPatternCount++;
            maxPatternCount++;
        }

        SetPattern();
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        animator.SetTrigger(hashOnDamage);
    }

    public void OnDie()
    {
        IsAlive = false;
        BossGenerator bossGen = GetComponentInParent<BossGenerator>();
        bossGen.IsBossGenerated = false;
        gameManager.UIManage.SetBossUI(IsAlive);
        gameManager.BossPool.ReturnObject(gameObject);
        gameManager.KillBossCount++;
        SoundManager.Inst.PlaySFX(SFXEnum.MonsterDie);
    }

    public void OnRestart()
    {
        minPatternCount = 2;
        maxPatternCount = 5;
        IsAlive = true;
    }

    private void SetPattern()
    {
        int count = Random.Range(minPatternCount, maxPatternCount);
        pattern = new Queue<int>(count);

        for (int i = 0; i < count; i++)
        {
            int result = Random.Range(-1, 1);
            if (result == 0) result = 1;
            pattern.Enqueue(result);
        }

        gameManager.UIManage.RefreshBossPattern(Pattern);
        isPatternSet = true;
    }
}