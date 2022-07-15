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

    public bool IsAlive
    {
        get => isAlive;
        set
        {
            isAlive = value;
            if(!isAlive)
                OnDie();
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
    }

    private void OnEnable()
    {
        Health = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            IAttackable target = col.gameObject.GetComponent<IAttackable>();
            target.TakeDamage(attack);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log($"{gameObject.name}의 남은 체력 : {Health}");
    }
    
    public void OnDie()
    {
        MonsterGenerator generator = GetComponentInParent<MonsterGenerator>();
        generator.IsMonsterSpawned = false;
        gameManager.MonPool.ReturnObject(gameObject, generator.MonsterIndex);
    }
}
