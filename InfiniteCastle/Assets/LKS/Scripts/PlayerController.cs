using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool isAlive = true;
    public float jumpSpeed = 3;

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnJumpInput(InputAction.CallbackContext callbackContext)
    {
        if (isAlive && callbackContext.performed)
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

        while (oldPos != newPos)
        {
            transform.position = Vector2.Lerp(oldPos, newPos, 1);
            yield return null;
        }

        CheckStair();
    }

    private void CheckStair()
    {
        RaycastHit info;

        Physics.Raycast(transform.position, Vector2.down, out info, 0.3f);
        if (!info.collider.CompareTag("Stair"))
            OnDie();
    }

    public void OnDie()
    {
        isAlive = false;
        Time.timeScale = 0;
    }
}
