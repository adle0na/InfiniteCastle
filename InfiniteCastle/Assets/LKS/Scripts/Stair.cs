using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField]
    private int thisFloor;

    [Header("몬스터 소환 관련")] [SerializeField] 
    private int monsterGenerateInterval = 6;
    [SerializeField] private int bossGenerateInterval = 10000;

    [Header("아이템 생성 관련")] [SerializeField] 
    private int itemGenInterval = 10;

    private int monsterIndex;

    private MonsterGenerator monsterGenerator;
    private ItemGenerator itemGenerator;

    public int ThisFloor
    {
        get => thisFloor;
        set
        {
            thisFloor = value;
            CheckMonGenerate();
        } 
    }

    public int MonsterIndex
    {
        get => monsterIndex;
        set
        {
            monsterIndex = SetGenerateMonsterIndex(value);
        }
    }

    public MonsterGenerator MonGenerator => monsterGenerator;
    public ItemGenerator ItemGen => itemGenerator;

    private void Awake()
    {
        monsterGenerator = GetComponentInChildren<MonsterGenerator>();
        itemGenerator = GetComponentInChildren<ItemGenerator>();
    }

    private int SetGenerateMonsterIndex(int newIndex)
    {
        int result = (newIndex % MonGenerator.monsters.Length);
        return result;
    }

    private void CheckMonGenerate()
    {
        if (thisFloor % itemGenInterval == 0)
        {
            itemGenerator.SetItem();
            return;
        }
        
        if (thisFloor % monsterGenerateInterval == 0)
        {
            MonGenerator.SpawnMonster(MonsterIndex);
        }
    }
}
