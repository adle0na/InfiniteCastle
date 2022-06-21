using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    private PlayerController player;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    public void MoveUp()
    {
        transform.Translate(new Vector3(0, 0.5f));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag($"Player"))
        {
            player.IsAlive = false;
        }
        else if (col.gameObject.CompareTag($"Stair"))
        {
            Destroy(col.gameObject);
        }
    }
}
