using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    private bool isBossGenerated = false;

    public GameObject bossPrefab;
    private GameManager gameManager;
    
    public bool IsBossGenerated
    {
        get => isBossGenerated;
        set => isBossGenerated = value;
    }

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void SetBoss()
    {
        //GameObject obj = Instantiate(bossPrefab, transform.position, Quaternion.identity);
        GameObject obj = gameManager.BossPool.GetObject();
        obj.transform.position = transform.position;
        obj.transform.parent = transform;
        IsBossGenerated = true;
    }
}
