using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IAttackable
{
    private int health;
    private int maxHealth = 5;
    private bool isAlive = true;
    private int attack = 2;

    private GameManager gameManager;
    private Animator animator;

    protected readonly int hashOnAttack = Animator.StringToHash("onAttack");
    protected readonly int hashIsAlive = Animator.StringToHash("isAlive");

    public bool IsAlive
    {
        get => isAlive;
        set
        {
            isAlive = value;
            if (!isAlive)
                OnDie();
            animator.SetBool(hashIsAlive, IsAlive);
        }
    }

    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, MaxHealth);
            if (health == 0)
                OnDie();
            onHpChange?.Invoke();
        }
    }

    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public int Attack
    {
        get => attack;
        set => attack = value;
    }

    public HpDelegate onHpChange { get; set; }

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        Health = maxHealth;
        IsAlive = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger(hashOnAttack);
            IAttackable target = col.gameObject.GetComponent<IAttackable>();
            target.TakeDamage(attack);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }

    public void OnDie()
    {
        animator.SetBool(hashIsAlive, false);
        MonsterGenerator generator = GetComponentInParent<MonsterGenerator>();
        generator.IsMonsterSpawned = false;
        gameManager.MonPool.ReturnObject(gameObject, generator.MonsterIndex);
        gameManager.KillMonsterCount++;
    }

    public void OnRestart()
    {
        maxHealth = 5;
        Health = maxHealth;
        attack = 2;
        isAlive = true;
    }
}