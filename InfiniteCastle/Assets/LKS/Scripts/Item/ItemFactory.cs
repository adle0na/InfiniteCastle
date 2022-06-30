using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public GameObject GetItem(ItemEnum ID)
    {
        GameObject itemObj = new GameObject();
        Item item = itemObj.AddComponent<Item>();
        item.itemEnum = ID;
        return itemObj;
    }
}
