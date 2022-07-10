using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour, IAttackable
{
    private int attack;
    private int health;
    private int maxHealth = 10;
    private bool isAlive = true;
    private bool isPatternSet = false;
    private Queue<int> pattern;
    
    public bool IsAlive
    {
        get => isAlive;
        set => isAlive = value;
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
            Debug.Log($"Boss Health : {Health}");
            if(health == 0)
                OnDie();
            else
                SetPattern();
        } 
    }
    public int MaxHealth
    {
        get => maxHealth;
    }
    public int Attack
    {
        get => attack;
        set => attack = value;
    }

    public Queue<int> Pattern => pattern;

    private void OnEnable()
    {
        Health = MaxHealth;
        Attack = 10;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }

    public void OnDie()
    {
        IsAlive = false;
        BossGenerator bossGen = GetComponentInParent<BossGenerator>();
        bossGen.IsBossGenerated = false;
        Destroy(gameObject);
    }

    private void SetPattern()
    {
        int count = Random.Range(4, 7);
        pattern = new Queue<int>(count);

        for (int i = 0; i < count; i++)
        {
            int result = Random.Range(-1, 1);
            if (result == 0) result = 1;
            pattern.Enqueue(result);
        }

        isPatternSet = true;
    }
}