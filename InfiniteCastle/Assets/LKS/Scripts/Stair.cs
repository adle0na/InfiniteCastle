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

    private int monsterIndex;

    private MonsterGenerator monsterGenerator;

    public int ThisFloor
    {
        get => thisFloor;
        set
        {
            thisFloor = value;
            CheckGenerate();
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

    private void Awake()
    {
        monsterGenerator = GetComponentInChildren<MonsterGenerator>();
    }

    private int SetGenerateMonsterIndex(int newIndex)
    {
        int result = (newIndex % MonGenerator.monsters.Length);
        return result;
    }

    private void CheckGenerate()
    {
        if (thisFloor % monsterGenerateInterval == 0)
        {
            MonGenerator.SpawnMonster(MonsterIndex);
            return;
        }
    }
}
