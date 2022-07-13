using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void HpDelegate();

public class PlayerController : MonoBehaviour, IAttackable
{
    #region Variables
    
    private bool isAlive = true;
    private bool isGrounded = false;
    private bool isAttackReady = false;

    private int fullInputCount;
    private int inputRound;

    private int attack;
    private int bombCount;
    private int maxBombCount = 3;
    private int health;
    private int maxHealth;

    public float jumpSpeed = 3;
    public int bombAttack = 50;

    private Vector2 oldPos;
    private Rigidbody2D rigidbody;
    private Killzone killZone;
    private Collider2D collider;
    private GameManager gameManager;

    private IEnumerator onMoveInput;
    public HpDelegate onHpChange;

    #endregion
    
    #region Properties

    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            if (health == 0)
                IsAlive = false;
            onHpChange.Invoke();
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
            {
                OnDie();
                gameManager.UIManage.SetGameOver(true);
            }
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
            bombCount = Mathf.Clamp(value, 0, maxBombCount);
            gameManager.UIManage.RefreshBombCount();
        }
    }
    public int MaxBombCount
    {
        get => maxBombCount;
    }

    #endregion
    
    #region Unity Built-In

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

    #endregion
    
    #region Move And Attack

    public void OnJumpInput(InputAction.CallbackContext callbackContext)
    {
        if (!isGrounded) return;
        if (IsAlive && callbackContext.performed)
        {
            StopAllCoroutines();
            Vector2 inputDir = callbackContext.ReadValue<Vector2>();

            //bool temp = CheckBoss();
            onMoveInput = CheckBoss() ? ResolvePattern(inputDir) : OnJump(inputDir);
            //StartCoroutine(OnJump(inputDir));
            StartCoroutine(onMoveInput);
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

    public void OnJumpInput(Vector2 dir)
    {
        if (!isGrounded) return;
        if (IsAlive)
        {
            StopAllCoroutines();
            
            onMoveInput = CheckBoss() ? ResolvePattern(dir) : OnJump(dir);
            StartCoroutine(onMoveInput);
        }
    }

    public void OnAttack()
    {
        if (BombCount == 0) return;
        if (IsAlive)
        {
            Monster[] monsters = GameObject.FindObjectsOfType<Monster>();
            foreach (var monster in monsters)
            {
                monster.TakeDamage(bombAttack);
            }

            BombCount--;
        }
    }

    #endregion
    
    #region Jump Methods

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
            killZone.MoveUp();
            CheckStair();
        }
    }

    private bool CheckMonster()
    {
        int nextFloor = gameManager.CurrentFloor;
        bool result = gameManager.CheckStair(++nextFloor);

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

    private bool CheckBoss()
    {
        bool result = false;

        int nextFloor = gameManager.CurrentFloor;
        if (gameManager.CheckBoss(++nextFloor))
        {
            result = true;
        }

        return result;
    }

    private void SetBossAttack()
    {
        if (isAttackReady) return;

        //bossAttackInput = new Queue<int>(gameManager.BossMonster.Pattern.Count);
        fullInputCount = gameManager.BossMonster.Pattern.Count;
        Debug.Log(
            $"Boss Pattern : {string.Join(" ", gameManager.BossMonster.Pattern.ToArray())}\nCount : {fullInputCount}");
        inputRound = 0;
        isAttackReady = true;
    }

    private IEnumerator ResolvePattern(Vector2 input)
    {
        if (!isAttackReady)
        {
            SetBossAttack();
            yield break;
        }
        if (!gameManager.BossMonster.IsPatternSet) yield break;

        inputRound = (input.x == gameManager.BossMonster.Pattern.Dequeue()) ? ++inputRound : 0;
        if (inputRound == fullInputCount)
        {
            AttackBoss(inputRound);
        }
        else if (inputRound == 0)
        {
            AttackBoss(inputRound);
            isAttackReady = false;
            SetBossAttack();
            TakeDamage(gameManager.BossMonster.Attack);
        }

        yield return null;
    }

    private void AttackBoss(int count)
    {
        Debug.Log("보스 공격!");
        IAttackable bossAttack = gameManager.BossMonster.GetComponent<IAttackable>();
        bossAttack.TakeDamage(count * Attack);
        isAttackReady = false;
        SetBossAttack();
    }

    #endregion

    #region IAttackable Methods

    public void TakeDamage(int damage)
    {
        Health -= damage;
        // 피격 이펙트
    }

    public void OnDie()
    {
        Time.timeScale = 0;
        StopAllCoroutines();
        collider.isTrigger = true;
    }

    #endregion
}