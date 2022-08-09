using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public int bombAttack = 10000;

    private Vector2 oldPos;
    private Rigidbody2D rigidbody;
    private Killzone killZone;
    private Collider2D collider;
    private GameManager gameManager;
    private Animator animator;

    private IEnumerator onMoveInput;

    protected readonly int hashOnJump = Animator.StringToHash("onJump");
    protected readonly int hashOnAttack = Animator.StringToHash("onAttack");
    protected readonly int hashOnDamage = Animator.StringToHash("onDamage");
    protected readonly int hashIsAlive = Animator.StringToHash("isAlive");

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

            animator.SetBool(hashIsAlive, IsAlive);
        }
    }
    public int Attack
    {
        get => attack;
        set => attack = value;
    }

    public HpDelegate onHpChange { get; set; }

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
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        IsAlive = true;
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
            SoundManager.Inst.PlaySFX(SFXEnum.Attack);
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
            
            onMoveInput = CheckBoss() ? ResolvePattern(inputDir) : OnJump(inputDir);
            StartCoroutine(onMoveInput);
        }
    }

    public void OnAttack(InputAction.CallbackContext callbackContext)
    {
        if (BombCount == 0) return;
        if (IsAlive && callbackContext.started)
        {
            SoundManager.Inst.PlaySFX(SFXEnum.Bomb);
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
            SoundManager.Inst.PlaySFX(SFXEnum.Bomb);
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
        SoundManager.Inst.PlaySFX(SFXEnum.Jump);
        
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
            animator.SetTrigger(hashOnJump);
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
        RaycastHit2D rayhit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("KillZone"));
        if (rayhit.collider.CompareTag("KillZone"))
        {
            animator.SetBool(hashIsAlive, isAlive);
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
        
        fullInputCount = gameManager.BossMonster.Pattern.Count;
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
            animator.SetTrigger(hashOnAttack);
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
        IAttackable bossAttack = gameManager.BossMonster.GetComponent<IAttackable>();
        bossAttack.TakeDamage(count * Attack);
        SoundManager.Inst.PlaySFX(SFXEnum.Attack);
        isAttackReady = false;
        SetBossAttack();
    }

    #endregion

    #region IAttackable Methods

    public void TakeDamage(int damage)
    {
        animator.SetTrigger(hashOnDamage);
        Health -= damage;
        // 피격 이펙트
    }

    public void OnDie()
    {
        SoundManager.Inst.PlaySFX(SFXEnum.PlayerDie);
        Time.timeScale = 0;
        StopAllCoroutines();
        collider.isTrigger = true;
    }

    #endregion

    public void OnRestart()
    {
        Health = maxHealth;
        BombCount = 1;
        IsAlive = true;
    }
}