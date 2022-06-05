using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairGenerator : MonoBehaviour
{
    public int initStairCount = 100;

    public GameObject stairPrefab;
    private GameObject[] stairs;

    private void Awake()
    {
        for(int i = 0; i < initStairCount; i++)
        {
            Instantiate(stairPrefab);
        }

        stairs = GameObject.FindGameObjectsWithTag("Stair");
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        int initPosX = Random.Range(-2, 3);
        float initPosY = -3.65f;
        Vector3 generatePos = new Vector3(initPosX, initPosY, 0);

        foreach(var stair in stairs)
        {
            stair.transform.position = generatePos;

            int nextX = Random.Range(-1, 1);
            if (nextX == 0)
                nextX = 1;
            if ((initPosX + nextX) == -3 || (initPosX + nextX) == 3)
                nextX *= -1;

            initPosX += nextX;
            initPosY += 0.5f;

            generatePos = new Vector3(initPosX, initPosY, 0);
        }
    }
}
