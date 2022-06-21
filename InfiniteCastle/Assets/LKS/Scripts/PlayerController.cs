using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool isAlive = true;
    public float jumpSpeed = 3;

    private Rigidbody2D rigidbody;
    private Killzone killZone;

    public bool IsAlive
    {
        get => isAlive;
        set
        {
            isAlive = value;
            if(!isAlive)
                OnDie();
        } 
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        killZone = GameObject.FindObjectOfType<Killzone>();
    }

    public void OnJumpInput(InputAction.CallbackContext callbackContext)
    {
        if (IsAlive && callbackContext.performed)
        {
            StopAllCoroutines();

            Vector2 inputDir = callbackContext.ReadValue<Vector2>();
            StartCoroutine(OnJump(inputDir));
        }
    }

    private IEnumerator OnJump(Vector2 inputDir) //나중에 델리게이트로 변경
    {
        Debug.Log("점프");
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
        Vector2 oldPos = transform.position;
        Vector2 newPos = new Vector2(transform.position.x + inputDir.x, transform.position.y);
        float distance = Vector2.Distance(oldPos, newPos);

        while (distance > 0.1f)
        {
            transform.position = Vector2.Lerp(oldPos, newPos, 1);
            distance = Vector2.Distance(transform.position, newPos);
            yield return null;
        }

        killZone.MoveUp();
        CheckStair();
    }

    private void CheckStair()
    {
        Debug.Log("바닥체크중");
        Debug.DrawRay(transform.position, Vector3.down, new Color(1, 0, 0));
        
        RaycastHit2D rayhit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("KillZone"));
        Debug.Log(rayhit.collider.gameObject.name);
        // if (!info.collider.CompareTag("Stair"))
        // {
        //     StopAllCoroutines();
        //     OnDie();
        // }
        if (rayhit.collider.CompareTag("KillZone"))
        {
            StopAllCoroutines();
            OnDie();
        }
    }

    public void OnDie()
    {
        Debug.Log("플레이어 사망");
        StopAllCoroutines();
        Time.timeScale = 0;
    }
}
