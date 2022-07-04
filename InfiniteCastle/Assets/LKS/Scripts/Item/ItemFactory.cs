using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    private ItemManager itemManager;

    private void Awake()
    {
        itemManager = GameObject.FindObjectOfType<ItemManager>();
    }

    public GameObject GetItem(ItemEnum ID)
    {
        GameObject obj = new GameObject();
        Item item = obj.AddComponent<Item>();
        item.itemEnum = ID;
        item.itemData = itemManager[ID];

        GameObject itemObj = Instantiate(item.itemData.itemPrefab, transform.position, Quaternion.identity);
        Destroy(obj);
        return itemObj;
    }
}
