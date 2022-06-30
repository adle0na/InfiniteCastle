using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IAttackable
{
    private int health;
    private int maxHealth;
    private bool isAlive = true;
    private int attack;

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
        }
    }
    public int MaxHealth => maxHealth;
    public int Attack
    {
        get => attack;
        set => attack = value;
    }

    private void Awake()
    {
        maxHealth = 5;
        Health = maxHealth;
        Attack = 2;
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
        Debug.Log($"{gameObject.name}을 죽였다!");
        Destroy(gameObject);
    }
}
