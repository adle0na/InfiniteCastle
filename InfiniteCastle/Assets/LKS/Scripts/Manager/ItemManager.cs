using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemData[] itemDatas;
    public ItemData this[int i]
    {
        get => itemDatas[i];
    }

    public ItemData this[ItemEnum i]
    {
        get => itemDatas[(int) i];
    }
}
