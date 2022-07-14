using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerController player;
    private StairGenerator stairManager;
    private Stair stair;
    private Stair[] stairs;
    private ItemFactory factory;
    private Boss bossMonster;
    private UIManager uiManager;
    private MemoryPool monPool;
    private MemoryPool bossPool;
    private MemoryPool itemPool;

    [SerializeField] private int currentFloor;
    [SerializeField] private int setNewMonster = 50;
    [SerializeField] private int healthRestore = 10;
    [SerializeField] private int hpRecovoery = 10;

    public PlayerController Player => player;
    public StairGenerator StairManager => stairManager;
    public Stair TheStair => stair;
    public ItemFactory Factory => factory;
    public Boss BossMonster => bossMonster;
    public UIManager UIManage => uiManager;
    public MemoryPool MonPool => monPool;
    public MemoryPool BossPool => bossPool;
    public MemoryPool ItemPool => itemPool;

    public int CurrentFloor
    {
        get => currentFloor;
        set
        {
            currentFloor = value;
            if((currentFloor % healthRestore)  == 0)
                RestoreHealth();
            if ((currentFloor % setNewMonster) == 0)
                SetIndex();
            uiManager.RefreshFloor();
        }
    }

    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        stairManager = GameObject.FindObjectOfType<StairGenerator>();
        factory = GameObject.FindObjectOfType<ItemFactory>();
        uiManager = GameObject.FindObjectOfType<UIManager>();
        monPool = GameObject.Find("MonsterMemoryPool").GetComponent<MemoryPool>();
        //bossPool = GameObject.Find("BossMemoryPool").GetComponent<MemoryPool>();
        itemPool = GameObject.Find("ItemMemoryPool").GetComponent<MemoryPool>();
    }
    
    private void Start()
    {
        stairs = GameObject.FindObjectsOfType<Stair>();
        //Test_SetItems();
        //Test_Boss();
    }
    
    public void SetPlayer(Vector3 stairPos)
    {
        float maxX = 2;
        if (stairPos.x == maxX)
        {
            Player.transform.position = stairPos + new Vector3(-1, 0, 0);
        }
        else
        {
            Player.transform.position = stairPos + new Vector3(1, 0, 0);
        }
    }

    public bool CheckStair(int thisFloor)
    {
        bool result = false;
        
        for (int i = 0; i < stairs.Length; i++)
        {
            if (stairs[i].ThisFloor == thisFloor)
            {
                stair = stairs[i];
                result = stairs[i].MonGenerator.IsMonsterSpawned;
            }
        }
        
        return result;
    }

    public bool CheckBoss(int thisFloor)
    {
        bool result = false;
        
        if (GameObject.FindObjectOfType<Boss>() != null)
        {
            bossMonster = GameObject.FindObjectOfType<Boss>();
            stair = bossMonster.GetComponentInParent<Stair>();
            if (stair.ThisFloor == thisFloor)
            {
                result = true;
                uiManager.SetBossUI(result);
            }
        }

        return result;
    }

    private void SetIndex()
    {
        if (CurrentFloor == 0) return;
        
        foreach (var stair in stairs)
        {
            stair.MonsterIndex++;
        }
        
        Monster monster = stair.MonGenerator.monsters[stair.MonsterIndex].GetComponent<Monster>();
        monster.MaxHealth *= 2;
        monster.Attack *= 2;
    }

    private void Test_SetItems()
    {
        foreach (var theStair in stairs)
        {
            if (theStair.ThisFloor == 1 || theStair.ThisFloor == 2)
            {
                if(!theStair.ItemGen.IsItemSet)
                    theStair.ItemGen.SetItem();
            }
        }
    }

    private void Test_Boss()
    {
        foreach (var theStair in stairs)
        {
            if (theStair.ThisFloor == 5)
            {
                theStair.BossGen.SetBoss();
            }
        }
    }

    private void RestoreHealth()
    {
        if(currentFloor == 0)  return;
        player.Health += hpRecovoery;
    }

    public void HealthUp()
    {
        hpRecovoery += 5;
        Debug.Log($"hpRecovoery : {hpRecovoery}");
    }

    public void AttackUp()
    {
        player.Attack += 1;
        Debug.Log($"Player Attack : {player.Attack}");
    }
}