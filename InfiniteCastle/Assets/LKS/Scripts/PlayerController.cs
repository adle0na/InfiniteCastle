using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IAttackable
{
    private bool isAlive = true;
    private bool isGrounded = false;
    private Vector2 oldPos;
    private int attack;
    private int bombCount;
    private int maxBombCount;
    
    public float jumpSpeed = 3;
    public int bombAttack = 50;

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
            if (!isAlive)
                OnDie();
        }
    }

    public int Attack
    {
        get => attack;
        set => attack = value;
    }

    public int BombCount
    {
        get => bombCount;
        set
        {
            bombCount = Mathf.Clamp(value, 0, maxHealth);
            Debug.Log($"남은 폭탄 갯수 : {BombCount}");
        }
    }

    public int MaxBombCount
    {
        get => maxBombCount;
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
        Attack = 1;
        maxHealth = 100;
        Health = MaxHealth;
        BombCount = 1;
    }

    private void Update()
    {
        RaycastHit2D rayhit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, LayerMask.GetMask("Stair"));
        if (rayhit.collider == null) return;
        if (rayhit.collider.CompareTag("Stair") || rayhit.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Monster"))
        {
            IAttackable target = col.gameObject.GetComponent<IAttackable>();
            target.TakeDamage(Attack);
            BounceOff();
        }
    }

    public void OnJumpInput(InputAction.CallbackContext callbackContext)
    {
        if (!isGrounded) return;
        if (IsAlive && callbackContext.performed)
        {
            StopAllCoroutines();

            Vector2 inputDir = callbackContext.ReadValue<Vector2>();
            StartCoroutine(OnJump(inputDir));
        }
    }

    public void OnAttack(InputAction.CallbackContext callbackContext)
    {
        if (BombCount == 0) return;
        if (IsAlive && callbackContext.started)
        {
            Monster[] monsters = GameObject.FindObjectsOfType<Monster>();
            foreach (var monster in monsters)
            {
                monster.TakeDamage(bombAttack);
            }
            BombCount--;
        }
    }

    private IEnumerator OnJump(Vector2 inputDir)
    {
        isGrounded = false;
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
        oldPos = transform.position;
        Vector2 newPos = new Vector2(transform.position.x + inputDir.x, transform.position.y);
        float distance = Vector2.Distance(oldPos, newPos);

        while (distance > 0.1f)
        {
            transform.position = Vector2.Lerp(oldPos, newPos, 1);
            distance = Vector2.Distance(transform.position, newPos);
            yield return null;
        }

        DoNext();
    }

    private void DoNext()
    {
        if (!CheckMonster())
        {
            Debug.Log("몬스터 없음");
            killZone.MoveUp();
            CheckStair();
        }
        else
        {
            Debug.Log("몬스터 있음");
        }
    }

    private bool CheckMonster()
    {
        int nextFloor = gameManager.CurrentFloor;
        bool result = gameManager.SetStair(++nextFloor);

        return result;
    }

    private void BounceOff()
    {
        transform.position = oldPos;
    }

    private void CheckStair()
    {
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

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log($"플레이어는 {damage}의 데미지를 입었다. \n 남은 체력 : {Health}");
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