using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isAlive = true;
    public float jumpSpeed = 3;

    [Header("이동 키매핑")] 
    public KeyCode leftJump = KeyCode.A;
    public KeyCode rightJump = KeyCode.D;
    
    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isAlive) return;

        if (Input.GetKeyDown(leftJump))
        {
            //LeftJump();
        }
        else if (Input.GetKeyDown(rightJump))
        {
            //RightJump();
        }
    }

    public void LeftJump()
    {
        Debug.Log("왼쪽점프");
    }

    public void RightJump()
    {
        Debug.Log("오른쪽점프");
    }

    public void OnDie()
    {
        isAlive = false;
        Time.timeScale = 0;
    }
}
