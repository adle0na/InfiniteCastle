using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairGenerator : MonoBehaviour
{
    public int initStairCount = 100;
    public GameObject stairPrefab;
    
    //private GameObject[] stairs;
    private Queue<GameObject> stairs;
    private Vector3 lastPosition;

    private void Awake()
    {
        stairs = new Queue<GameObject>(initStairCount);
        
        for(int i = 0; i < initStairCount; i++)
        {
            GameObject obj = Instantiate(stairPrefab, transform);
            stairs.Enqueue(obj);
        }

        //stairs = GameObject.FindGameObjectsWithTag("Stair");
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
        GenerateStairs(generatePos);
    }

    private void AddStairs()
    {
        GenerateStairs(lastPosition);
    }

    private void GenerateStairs(Vector3 generatePos)
    {
        foreach (var stair in stairs)
        {
            stair.transform.position = generatePos;

            int nextX = Random.Range(-1, 1);
            if (nextX == 0)
                nextX = 1;
            if ((generatePos.x + nextX) == -3 || (generatePos.x + nextX) == 3)
                nextX *= -1;

            generatePos.x += nextX;
            generatePos.y += 0.5f;

            generatePos = new Vector3(generatePos.x, generatePos.y, 0);
        }
        
        lastPosition = generatePos;
    }
}
