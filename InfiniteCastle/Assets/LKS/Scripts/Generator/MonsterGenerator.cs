using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    private bool isMonsterSpawned = false;
    
    public GameObject[] monsters;
    public bool IsMonsterSpawned
    {
        get => isMonsterSpawned;
        set => isMonsterSpawned = value;
    }

    public void SpawnMonster(int index)
    {
        GameObject obj = Instantiate(monsters[index], transform.position, Quaternion.identity);
        Monster monster = obj.GetComponent<Monster>();
        monster.Attack = 2 * (index + 1);
        obj.transform.parent = this.transform;
        isMonsterSpawned = true;
    }
}
