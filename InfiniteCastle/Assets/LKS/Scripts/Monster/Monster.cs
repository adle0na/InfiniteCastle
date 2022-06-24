using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IAttackable
{
    private int health;
    private int maxHealth;
    private bool isAlive = true;

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
                IsAlive = false;
        }
    }
    public int MaxHealth
    {
        get => maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
    }

    public void OnDie()
    {
        
    }
}
