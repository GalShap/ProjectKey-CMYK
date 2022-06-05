using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    #region Inspector

    [Header("Running Physics")] 
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float deceleration = 10;
    [SerializeField] private float acceleration = 10;
    
    [Header("Jumping Physics")]
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float jumpTime = 1f;
    [SerializeField] private float fallMultiplier = 1f;
    [SerializeField] private float jumpDelay = 0.25f;
    private float jumpTimer;
    private bool jumping;
    
    [Header("Collision")]
    [SerializeField] private float collisionEps;
    private Vector2 collisionOffset;

    [Header("Attack")] 
    [SerializeField] private float attackTimer;
    [SerializeField] private GameObject attackRange;
    [SerializeField] private float attackRadius = 0.5f;
    private float attackCounter = 0;

    [Header("Other")]
    [SerializeField] private bool isTutorial;

    #endregion

    #region Fields

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _renderer;
    private Animator _animator;
    
    public static bool onGround;
    public static bool jumpAttacking = false; 
    private Vector2 movement;
    private Vector2 height;
    private bool jumpHit;

    private static Vector2? kickbackVector2 = null;
    
    #endregion

    #region Constants
    
    private const float IDLE = 0;

    private const float WALKING = 1f;
    
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Action = Animator.StringToHash("Action");
    private static readonly int Jump1 = Animator.StringToHash("jumping");
 
    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat(Action,IDLE);
        collisionOffset = Vector2.right * ((_renderer.sprite.rect.width/_renderer.sprite.pixelsPerUnit) / 2 - collisionEps);
    }

    private void Update()
    {
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
        
        height = Vector2.up * _renderer.sprite.rect.height / _renderer.sprite.pixelsPerUnit;
        collisionOffset = Vector2.right * ((_renderer.sprite.rect.width/_renderer.sprite.pixelsPerUnit) / 2 - collisionEps);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_rigidbody2D.position + collisionOffset - height*0.5f,
            Vector2.down*0.25f);
        Gizmos.DrawRay(_rigidbody2D.position - collisionOffset - height*0.5f,
            Vector2.down*0.25f);
        Gizmos.DrawWireSphere(attackRange.transform.position, attackRadius);
    }

    private void FixedUpdate()
    {
      
        
        RaycastHit2D hitr;
        RaycastHit2D hitl;
        hitr = Physics2D.Raycast(
            _rigidbody2D.position + collisionOffset - height*0.5f,
            Vector2.down,
            0.25f,
            ColorManager.GroundLayers); 
                    
        hitl = Physics2D.Raycast(
            _rigidbody2D.position - collisionOffset - height*0.5f,
            Vector2.down,
            0.25f,
            ColorManager.GroundLayers);
        
        bool checkGround = hitl || hitr;
        if (!onGround && checkGround)
        {
            bool hitMonster =
                hitl && (hitl.collider.CompareTag("Monster") || hitl.collider.gameObject.CompareTag("Projectile")) ||
                hitr && (hitr.collider.CompareTag("Monster") || hitr.collider.gameObject.CompareTag("Projectile"));
            if (!hitMonster)
            {
                _animator.SetBool(Jump1, false);
                if (jumpAttacking)
                {
                    jumpAttacking = false;
                    jumpHit = false;
                }
                onGround = checkGround;
            }
        }
        if(!jumpAttacking) 
            onGround = checkGround;
        if (jumpTimer > Time.time && onGround)
        {
            Jump();
        }


        MoveCharacter(); 
        ModifyPhysics();
    }

    public void ToIdle()
    {
        _animator.SetFloat(Action, IDLE);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Respawn"))
        {
            GameManager.Manager.SetRespawn(other.gameObject);
        }
    }

    #endregion

    #region Private Methods

    private void MoveCharacter()
    {
      
        var desiredVel = new Vector2(movement.x * moveSpeed, _rigidbody2D.velocity.y);
        
        if (kickbackVector2 != null)
        {   
           
                desiredVel =  kickbackVector2.Value;
                _rigidbody2D.AddForce(desiredVel, ForceMode2D.Impulse);
            kickbackVector2 = null;
        }
        else
        {
            if (!jumpHit)
            {
                _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, 
                    desiredVel, 
                    acceleration * Time.fixedDeltaTime);   
            }

            if (Math.Abs(_rigidbody2D.velocity.x) > maxSpeed)
            {
                float sign = Mathf.Sign(_rigidbody2D.velocity.x);
                _rigidbody2D.velocity = new Vector2(sign * maxSpeed, _rigidbody2D.velocity.y);
            }
        }
    }

    private void Jump()
    {   
        AudioManager.SharedAudioManager.PlayKeyActionSound((int) AudioManager.KeySounds.Jump);
        _animator.SetBool(Jump1,true);
        _rigidbody2D.drag = 0;
        float y = (2 * jumpHeight) / jumpTime;
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, y);
        jumpTimer = 0;
     
    }

    private void ModifyPhysics()
    {
        bool changingDirection = 0 > movement.x * _rigidbody2D.velocity.x;
        if (onGround)
        {
            if ((Math.Abs(movement.x) < 0.4 && _rigidbody2D.velocity.x != 0) || changingDirection)
            {
                // _rigidbody2D.drag = linearDrag;
                _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, 
                    _rigidbody2D.velocity * Vector2.up, 
                    deceleration * Time.fixedDeltaTime);
            }
            if (Math.Abs(movement.x) == 0 && Math.Abs(_rigidbody2D.velocity.x) < 0.2f)
            {
                _rigidbody2D.velocity *= Vector2.up;
            }
            else
            {
                _rigidbody2D.drag = 0f;
            }
        }
        else
        {
            _rigidbody2D.drag = 0;
            float scale = (-2 * jumpHeight) / (Mathf.Pow(jumpTime, 2));
            if (_rigidbody2D.velocity.y > 0 && !jumping)
            {
                scale *= fallMultiplier;
            }

            _rigidbody2D.gravityScale = scale / Physics2D.gravity.y;
        }
    }
    
    public void StartAttack()
    {
        var dir = attackRange.transform.position - transform.position;
        var dir2 = new Vector2(dir.x, dir.y);

        var hits = Physics2D.CircleCastAll(attackRange.transform.position, attackRadius, dir2, 0.05f);
        foreach (var h in hits)
        {
            BlueGod god = h.collider.GetComponent<BlueGod>();
            if (god != null)
            {
                god.Hit();
                return;
            }
            
            EnemyHealth enemy = h.collider.GetComponent<EnemyHealth>();
            if (enemy == null) continue;
            enemy.Hit(gameObject);
        }
    }

    public void Stop()
    {
        movement = Vector2.zero;
    }

    #endregion

    #region On Input

    public void onAttack(InputAction.CallbackContext context)
    {
        if (TimelineManager.Manager.IsPlaying)
            return;
        
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if (isTutorial && TutorialManager.Manager.State == TutorialManager.TutorialState.ATTACK)
                {
                    TutorialManager.Manager.HideTutorial();
                    TutorialManager.Manager.SetState(TutorialManager.TutorialState.COLOR);
                }
                if (attackCounter <= 0)
                {
                   if(jumpAttacking) return;
                    
                    AudioManager.SharedAudioManager.PlayKeyActionSound((int)AudioManager.KeySounds.Attack);
                    _animator.SetTrigger(Attack);
                    attackCounter = attackTimer;
                    if (!onGround)
                        jumpAttacking = true;

                }
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (TimelineManager.Manager.IsPlaying)
            return;
        
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if (isTutorial && TutorialManager.Manager.State == TutorialManager.TutorialState.JUMP)
                {
                    TutorialManager.Manager.HideTutorial();
                    TutorialManager.Manager.SetState(TutorialManager.TutorialState.ATTACK);
                }
                jumpTimer = Time.time + jumpDelay;
                jumping = true;
                break;
            case InputActionPhase.Canceled:
                jumping = false;
                break;
        }
    }

    public void onMove(InputAction.CallbackContext context)
    {
        if (TimelineManager.Manager.IsPlaying)
            return;
        
        switch (context.phase)
        {
            case InputActionPhase.Performed:

                var input = context.ReadValue<Vector2>();
                if(Math.Abs(input.x) <= 0.1)
                    return;
                
                if (isTutorial && TutorialManager.Manager.State == TutorialManager.TutorialState.MOVE)
                {
                    TutorialManager.Manager.HideTutorial();
                    TutorialManager.Manager.SetState(TutorialManager.TutorialState.JUMP);
                }

                movement = input;
                
                bool facingRight = _renderer.flipX;
                
                //change attack collider location
                bool changingDir = facingRight != movement.x <= 0;
                if (changingDir)
                {
                    // var pos = attackCollider.gameObject.transform.localPosition;
                    // pos.x *= -1;
                    // attackCollider.gameObject.transform.localPosition = pos;
                    var pos = attackRange.transform.localPosition;
                    pos.x *= -1;
                    attackRange.transform.localPosition = pos;
                }

                _renderer.flipX = movement.x <= 0;
                _animator.SetFloat(Action, WALKING);
                
                break;
            case InputActionPhase.Canceled:
                movement = Vector2.zero;
                _animator.SetFloat(Action, IDLE);
                break;
        }
    }

    public void OnChangeColor(InputAction.CallbackContext context)
    {
        if (TimelineManager.Manager.IsPlaying)
            return;
        
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if (isTutorial)
                {
                    if (TutorialManager.Manager.State == TutorialManager.TutorialState.COLOR 
                        || TutorialManager.Manager.State == TutorialManager.TutorialState.END)
                    {
                        TutorialManager.Manager.HideTutorial();
                        TutorialManager.Manager.SetState(TutorialManager.TutorialState.END);   
                    }
                    else
                    {
                        return;
                    }
                }
                ColorManager.RotateColor(1);
                AudioManager.SharedAudioManager.PlayKeyActionSound((int) AudioManager.KeySounds.ColorSwitch);
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }
    
    public void OnRotateColorLeft(InputAction.CallbackContext context)
    {
        if (TimelineManager.Manager.IsPlaying)
            return;
        
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if (isTutorial)
                {
                    if (TutorialManager.Manager.State == TutorialManager.TutorialState.COLOR 
                        || TutorialManager.Manager.State == TutorialManager.TutorialState.END)
                    {
                        TutorialManager.Manager.HideTutorial();
                        TutorialManager.Manager.SetState(TutorialManager.TutorialState.END);   
                    }
                    else
                    {
                        return;
                    }
                }
                ColorManager.RotateColorRight();
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }
    
    public void OnRotateColorRight(InputAction.CallbackContext context)
    {
        if (TimelineManager.Manager.IsPlaying)
            return;
        
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if (isTutorial)
                {
                    if (TutorialManager.Manager.State == TutorialManager.TutorialState.COLOR 
                        || TutorialManager.Manager.State == TutorialManager.TutorialState.END)
                    {
                        TutorialManager.Manager.HideTutorial();
                        TutorialManager.Manager.SetState(TutorialManager.TutorialState.END);   
                    }
                    else
                    {
                        return;
                    }
                }
                ColorManager.RotateColorLeft();
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }

    #endregion


    
   
    public static void SetKickBack(Vector2 kickback)
    {
        kickbackVector2 = kickback;
    }


    public void JumpHit()
    {
        jumpHit = true;
    }
}
