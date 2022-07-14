using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    private bool isMonsterSpawned = false;
    private int monsterIndex;
    
    public GameObject[] monsters;
    private GameManager gameManager;

    public int MonsterIndex
    {
        get => monsterIndex;
        set => monsterIndex = value;
    }

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public bool IsMonsterSpawned
    {
        get => isMonsterSpawned;
        set => isMonsterSpawned = value;
    }

    public void SpawnMonster(int index)
    {
        MonsterIndex = index;

        GameObject obj = gameManager.MonPool.GetObject(index);
        obj.transform.position = this.transform.position;
        obj.transform.parent = this.transform;
        isMonsterSpawned = true;
    }
}
