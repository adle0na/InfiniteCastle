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
        gameManager.ItemPool.ReturnObject(gameObject, itemGenerator.Index);
    }

    private void GetHealthUp()
    {
        gameManager.HealthUp();
        gameManager.ItemPool.ReturnObject(gameObject, itemGenerator.Index);
    }

    private void GetBomb()
    {
        ++gameManager.Player.BombCount;
        gameManager.ItemPool.ReturnObject(gameObject, itemGenerator.Index);
    }
}
