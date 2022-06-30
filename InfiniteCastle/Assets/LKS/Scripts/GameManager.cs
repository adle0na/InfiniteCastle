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

    [SerializeField] private int currentFloor;
    [SerializeField] private int setNewMonster = 50;
    [SerializeField] private int healthRestore = 10;
    [SerializeField] private int hpRecovoery = 10;

    public PlayerController Player => player;
    public StairGenerator StairManager => stairManager;
    public Stair TheStair => stair;
    public ItemFactory Factory => factory;

    public int CurrentFloor
    {
        get => currentFloor;
        set
        {
            currentFloor = value;
            Debug.Log(currentFloor);
            if((currentFloor % healthRestore)  == 0)
                RestoreHealth();
            if ((currentFloor % setNewMonster) == 0)
                SetIndex();
        }
    }

    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        stairManager = GameObject.FindObjectOfType<StairGenerator>();
        factory = GameObject.FindObjectOfType<ItemFactory>();
    }
    
    private void Start()
    {
        stairs = GameObject.FindObjectsOfType<Stair>();
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

    public bool SetStair(int thisFloor)
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

    private void SetIndex()
    {
        if (CurrentFloor == 0) return;
        
        foreach (var stair in stairs)
        {
            stair.MonsterIndex++;
        }
        
        Debug.Log("몬스터 변경");
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
        player.Attack++;
        Debug.Log($"Player Attack : {player.Attack}");
    }
}