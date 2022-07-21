using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    #region Variables
    
    private PlayerController player;
    private StairGenerator stairManager;
    private Stair stair;
    private Stair[] stairs;
    private Boss bossMonster;
    private UIManager uiManager;
    private MemoryPool monPool;
    private MemoryPool bossPool;
    private MemoryPool itemPool;

    [SerializeField] private int currentFloor;
    [SerializeField] private int setNewMonster = 50;
    [SerializeField] private int healthRestore = 10;
    [SerializeField] private int hpRecovery = 10;
    private int killMonsterCount = 0;
    private int killBossCount = 0;

    #endregion
    
    #region Properties
    
    public PlayerController Player => player;
    public StairGenerator StairManager => stairManager;
    public Stair TheStair => stair;
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

    public int HpRecovery
    {
        get => hpRecovery;
        set => hpRecovery = Mathf.Clamp(value, 10, 20);
    }
    
    public int KillMonsterCount
    {
        get => killMonsterCount;
        set => killMonsterCount = value;
    }
    public int KillBossCount
    {
        get => killBossCount;
        set => killBossCount = value;
    }
    
    #endregion

    #region Unity Built_In

    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        stairManager = GameObject.FindObjectOfType<StairGenerator>();
        uiManager = GameObject.FindObjectOfType<UIManager>();
        monPool = GameObject.Find("MonsterMemoryPool").GetComponent<MemoryPool>();
        bossPool = GameObject.Find("BossMemoryPool").GetComponent<MemoryPool>();
        itemPool = GameObject.Find("ItemMemoryPool").GetComponent<MemoryPool>();
    }
    
    private void Start()
    {
        stairs = GameObject.FindObjectsOfType<Stair>();
        //Test_SetItems();
        //Test_Boss();
    }

    private void OnEnable()
    {
        KillMonsterCount = 0;
        KillBossCount = 0;
    }

    #endregion
    
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

        int index = 0;
        
        foreach (var stair in stairs)
        {
            stair.MonsterIndex++;
            index = stair.MonsterIndex;
        }

        for (int i = 0; i < monPool.ObjPools[index].Count; i++)
        {
            Monster monster = monPool.ObjPools[index].ElementAt(i).GetComponent<Monster>();
            monster.MaxHealth += 3;
            monster.Attack += 2;            
        }
    }

    public void OnRestart()
    {
        player.OnRestart();
        for (int j = 0; j < MonPool.ObjPools.Length; j++)
        {
            for (int i = 0; i < MonPool.ObjPools[j].Count; i++)
            {
                Monster monster = MonPool.ObjPools[j].ElementAt(i).GetComponent<Monster>();
                monster.OnRestart();
            }
        }

        bossMonster = BossPool.ObjPools[0].ElementAt(0).GetComponent<Boss>();
        bossMonster.OnRestart();
        
        killMonsterCount = 0;
        killBossCount = 0;

        Time.timeScale = 1;
    }

    #region Item Methods
    
    private void RestoreHealth()
    {
        if(currentFloor == 0)  return;
        player.Health += HpRecovery;
    }

    public void HealthUp()
    {
        HpRecovery += 3;
        Debug.Log($"hpRecovoery : {HpRecovery}");
    }

    public void AttackUp()
    {
        player.Attack += 1;
        Debug.Log($"Player Attack : {player.Attack}");
    }
    
    #endregion
    
    #region Test Methods

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

    #endregion
}