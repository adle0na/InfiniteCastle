using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StairGenerator : MonoBehaviour
{
    public int initStairCount;
    public GameObject stairPrefab;

    public Queue<GameObject> stairs;
    private Vector3 lastPosition;
    private GameManager gameManager;
    private int stairCount;

    private void Awake()
    {
        stairs = new Queue<GameObject>(initStairCount);
        gameManager = GameObject.FindObjectOfType<GameManager>();
        
        for(int i = 0; i < initStairCount; i++)
        {
            GameObject obj = Instantiate(stairPrefab, transform);
            Stair stairObj = obj.GetComponent<Stair>();
            stairObj.ThisFloor = i + 1;
            stairCount = stairObj.ThisFloor;
            stairs.Enqueue(obj);
        }
    }

    private void Start()
    {
        Initialize();
        gameManager.SetPlayer(stairs.ElementAt(0).gameObject.transform.position);
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

    public void RemoveStairs(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.position = transform.position;
        ResetStairs(obj, lastPosition);
    }

    private void ResetStairs(GameObject stair, Vector3 newPos)
    {
        Debug.Log("계단 재배치 계산중");
        stair.transform.position = newPos;
        
        int nextX = Random.Range(-1, 1);
        if (nextX == 0)
            nextX = 1;
        if ((newPos.x + nextX) == -3 || (newPos.x + nextX) == 3)
            nextX *= -1;
        
        newPos.x += nextX;
        newPos.y += 0.5f;
        lastPosition = newPos;
        
        stair.SetActive(true);
        
        Stair stairObj = stair.GetComponent<Stair>();
        stairCount++;
        stairObj.ThisFloor = stairCount;
        Debug.Log("계단 재배치 완료");
    }
}
