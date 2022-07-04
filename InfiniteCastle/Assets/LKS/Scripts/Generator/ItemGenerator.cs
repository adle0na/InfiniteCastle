using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemGenerator : MonoBehaviour
{
    private ItemFactory itemFactory;
    private bool isItemSet = false;

    [Header("아이템 확률")] public float attackUpRate = 0.2f;
    public float healthUpRate = 0.4f;
    public float bombRate = 0.4f;

    public bool IsItemSet
    {
        get => isItemSet;
        set => isItemSet = value;
    }

    private void Awake()
    {
        itemFactory = GameObject.FindObjectOfType<ItemFactory>();
    }

    public void SetItem()
    {
        int itemIndex = SetItemNumber();
        GameObject obj = itemFactory.GetItem((ItemEnum) itemIndex);
        obj.transform.position = transform.position;
        obj.transform.parent = transform;
        IsItemSet = true;
    }

    private int SetItemNumber()
    {
        int result;
        
        float random = Random.Range(0f, 1f);
        if (random < attackUpRate)
            result = 1;
        else if (random < (attackUpRate + healthUpRate))
            result = 2;
        else
            result = 3;

        return result;
    }
}