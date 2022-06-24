using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IAttackable
{
    private bool isAlive = true;
    private Vector2 moveDir = Vector2.zero;
    public float jumpSpeed = 3;

    private Rigidbody2D rigidbody;
    private Killzone killZone;
    private Collider2D collider;
    private GameManager gameManager;

    private int health;
    private int maxHealth;
    
    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            if (health == 0)
                IsAlive = false;
        } 
    }

    public int MaxHealth => maxHealth;

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
        collider = GetComponent<Collider2D>();
        killZone = GameObject.FindObjectOfType<Killzone>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        gameManager.CurrentFloor = 0;
    }

    public void OnJumpInput(InputAction.CallbackContext callbackContext)
    {
        if (IsAlive && callbackContext.performed)
        {
            StopAllCoroutines();

            Vector2 inputDir = callbackContext.ReadValue<Vector2>();
            //moveDir = callbackContext.ReadValue<Vector2>();
            StartCoroutine(OnJump(inputDir));
            //CheckMonster(moveDir);
        }
    }

    private bool CheckMonster(Vector2 inputDir)
    {
        bool result = false;
        Debug.Log("몬스터 존재 유무 확인중");
        Debug.DrawRay(transform.position, (Vector2.up + inputDir), Color.cyan);

        RaycastHit2D rayHit =
            Physics2D.Raycast(transform.position, (Vector2.up + inputDir), 1.5f, 8);

        if (rayHit == null)
        {
            StartCoroutine(OnJump(moveDir));
            return result;
        }
        else
        {
            Debug.Log(rayHit.collider.gameObject.name);
            result = true;
        }
        
        return result;
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
        if (rayhit.collider.CompareTag("KillZone"))
        {
            StopAllCoroutines();
            OnDie();
            return;
        }

        gameManager.CurrentFloor++;
    }

    private void CheckMonster1()
    {
        //이부분은 정상 작동 확인 후 지울 것
        Debug.Log("몬스터 존재 유무 확인중");
        Debug.DrawRay(transform.position, new Vector3(1,1,1), Color.cyan);
        Debug.DrawRay(transform.position, new Vector3(-1,1,1), Color.cyan);

        RaycastHit2D leftRayHit =
            Physics2D.Raycast(transform.position, new Vector2(-1, 1), 1.5f, 8);
        RaycastHit2D rightRayHit =
            Physics2D.Raycast(transform.position, new Vector2(1, 1), 1.5f, 8);

        if (leftRayHit != null)
            if (leftRayHit.collider.CompareTag("Monster"))
                Debug.Log(leftRayHit.collider.gameObject.name);
        else if (rightRayHit != null)
            if (rightRayHit.collider.CompareTag("Monster"))
                Debug.Log(rightRayHit.collider.gameObject.name);
    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
        // 피격 이펙트
    }

    public void OnDie()
    {
        Time.timeScale = 0;
        Debug.Log("플레이어 사망");
        StopAllCoroutines();
        collider.isTrigger = true;
    }
}
