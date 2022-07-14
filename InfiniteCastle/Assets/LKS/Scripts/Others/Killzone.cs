using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    private PlayerController player;
    private GameManager gameManager;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void MoveUp()
    {
        transform.Translate(new Vector3(0, 0.5f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag($"Player"))
        {
            player.IsAlive = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag($"Stair"))
        {
            gameManager.StairManager.RemoveStairs(other.gameObject);
        }
    }
}
