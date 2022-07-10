using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    private bool isBossGenerated = false;

    public GameObject bossPrefab;
    
    public bool IsBossGenerated
    {
        get => isBossGenerated;
        set => isBossGenerated = value;
    }

    public void SetBoss()
    {
        GameObject obj = Instantiate(bossPrefab, transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        IsBossGenerated = true;
    }
}
