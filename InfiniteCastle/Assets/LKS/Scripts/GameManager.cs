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
    [SerializeField] private int currentFloor;
    private int setNewMonster = 50;

    public PlayerController Player => player;
    public StairGenerator StairManager => stairManager;

    public Stair TheStair => stair;

    public int CurrentFloor
    {
        get => currentFloor;
        set
        {
            currentFloor = value;
            Debug.Log(currentFloor);
            if ((currentFloor % setNewMonster) == 0)
                SetIndex();
        }
    }

    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        stairManager = GameObject.FindObjectOfType<StairGenerator>();
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
}
