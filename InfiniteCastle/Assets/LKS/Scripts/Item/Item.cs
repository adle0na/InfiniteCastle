using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemEnum itemEnum = ItemEnum.Dummy;
    public ItemData itemData;
    private GameManager gameManager;
    private ItemGenerator itemGenerator;

    private void OnEnable()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        itemGenerator = GetComponentInParent<ItemGenerator>();
        itemGenerator.IsItemSet = false;
        
        if (col.CompareTag("Player"))
        {
            switch (itemEnum)
            {
                case ItemEnum.Dummy:
                    break;
                case ItemEnum.AttackUp:
                    GetAttackUp();
                    break;
                case ItemEnum.HealthUp:
                    GetHealthUp();
                    break;
                case ItemEnum.Bomb:
                    GetBomb();
                    break;
            }
        }
    }
    
    private void GetAttackUp()
    {
        gameManager.AttackUp();
        Destroy(gameObject);
    }

    private void GetHealthUp()
    {
        gameManager.HealthUp();
        Destroy(gameObject);
    }

    private void GetBomb()
    {
        ++gameManager.Player.BombCount;
        Destroy(gameObject);
    }
}
