using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MemoryPool : MonoBehaviour
{
    private int poolCount = 4;
    private int indexNumber;

    public GameObject[] objPrefabs;
    private Queue<GameObject>[] objPools;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        objPools = new Queue<GameObject>[objPrefabs.Length];
        indexNumber = 0;
        for (int i = 0; i < objPrefabs.Length; i++)
        {
            SetPool(poolCount, i);
        }
    }

    private void SetPool(int count, int index = 0)
    {
        objPools[index] = new Queue<GameObject>(count);
        indexNumber = 0;
        for (int j = 0; j < count; j++)
        {
            GameObject obj = Instantiate(objPrefabs[index], transform.position, Quaternion.identity);
            obj.transform.parent = transform;
            obj.gameObject.name = $"{objPrefabs[index].name}_{indexNumber}";
            obj.SetActive(false);
            objPools[index].Enqueue(obj);

            indexNumber++;
        }
    }

    public GameObject GetObject(int index = 0)
    {
        if (objPools[index].Count == 0) SetPool(poolCount << 1, index);

        GameObject obj = objPools[index].Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(int index, GameObject returnObj)
    {
        returnObj.transform.position = this.transform.position;
        returnObj.transform.parent = this.transform;
        returnObj.SetActive(false);
        objPools[index].Enqueue(returnObj);
    }
}