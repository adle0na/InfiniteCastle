using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Item", menuName = "Scriptable Object/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    public uint itemID;
    public string itemName = "아이템 이름";
    public Sprite itemImage;
    public string itemDescription = "아이템 설명";
    public GameObject itemPrefab;
    public bool isPassive;
}
