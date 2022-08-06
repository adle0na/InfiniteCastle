using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private GameObject[] backgrounds;

    private const float bgSize = 13.19f;
    [SerializeField] private int index = 0;
    [SerializeField] private int count;

    private void Start()
    {
        count = transform.childCount;
        backgrounds = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
        }
    }

    public void Scroll()
    {
        Debug.Log("Scroll");
        float scrollGap = bgSize * count;
        backgrounds[index].transform.position =
            new Vector3(0, backgrounds[index].transform.position.y + scrollGap, 0);
        index = (index + 1) % count;
    }
}