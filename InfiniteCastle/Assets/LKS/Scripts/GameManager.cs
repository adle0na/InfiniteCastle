using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerController player;
    private StairGenerator stairManager;
    private Stair[] stairs;
    [SerializeField] private int currentFloor;

    public PlayerController Player => player;
    public StairGenerator StairManager => stairManager;

    public Stair[] Stairs
    {
        get => stairs;
        set => stairs = value;
    }

    public int CurrentFloor
    {
        get => currentFloor;
        set
        {
            currentFloor = value;
            Debug.Log(currentFloor);
        }
    }

    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        stairManager = GameObject.FindObjectOfType<StairGenerator>();
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
}
