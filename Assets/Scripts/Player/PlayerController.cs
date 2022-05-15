using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Running Physics")] 
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float linearDrag = 5f;
    
    [Header("Jumping Physics")]
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float fallMultiplier = 4f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float jumpDelay = 0.25f;
    private float jumpTimer;
    private bool jumping;
    
    [Header("Collision")]
    [SerializeField] private LayerMask groundLayers;
    private Vector2 collisionOffset;

    [Header("Attack")] 
    [SerializeField] private float attackTimer;
    [SerializeField] private Collider2D attackCollider;
    private float attackCounter = 0;

    [Header("Other")] [SerializeField] private bool isTutorial;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _renderer;
    private Animator _animator;
    
    private bool onGround;
    private Vector2 movement;
   
    
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Action = Animator.StringToHash("Action");
    
    #region Constants
    
    private const float IDEAL = 0;

    private const float WALKING = 1f;
 
    #endregion

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        collisionOffset = Vector2.right * transform.lossyScale.x / 2;
    }

    private void Update()
    {
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        float height = _renderer.sprite.rect.height / _renderer.sprite.pixelsPerUnit; 
        onGround = Physics2D.Raycast(
                       _rigidbody2D.position + collisionOffset,
                       Vector2.down,
                        height * 0.5f + 0.05f,
                       groundLayers) 
                   || 
                   Physics2D.Raycast(
                       _rigidbody2D.position - collisionOffset,
                       Vector2.down,
                       height * 0.5f + 0.05f,
                       groundLayers);
        
        if (jumpTimer > Time.time && onGround)
        {
            Jump();
        }
        MoveCharacter();
        ModifyPhysics();
    }

    private void MoveCharacter()
    {
        _rigidbody2D.AddForce(Vector2.right * movement.x * moveSpeed);
        
        if (Math.Abs(_rigidbody2D.velocity.x) > maxSpeed)
        {
            float sign = Mathf.Sign(_rigidbody2D.velocity.x);
            _rigidbody2D.velocity = new Vector2(sign * maxSpeed, _rigidbody2D.velocity.y);
        }
    }

    private void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        _rigidbody2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
    }

    private void ModifyPhysics()
    {
        bool changingDirection = 0 > movement.x * _rigidbody2D.velocity.x;
        if (onGround)
        {
            if (Math.Abs(movement.x) < 0.4 || changingDirection)
            {
                _rigidbody2D.drag = linearDrag;
            }
            else
            {
                _rigidbody2D.drag = 0f;
            }
            
            // _rigidbody2D.gravityScale = 0;
        }
        else
        {
            _rigidbody2D.drag = 0;
            float scale = gravityScale;
            
            if (_rigidbody2D.velocity.y < 0)
            {
                scale *= fallMultiplier;
            }
            else if (_rigidbody2D.velocity.y > 0 && !jumping)
            {
                scale *= fallMultiplier / 2;
            }

            _rigidbody2D.gravityScale = scale;
        }
    }

    public void EndAttack()
    {
        attackCollider.gameObject.SetActive(false);
    }

    public void Stop()
    {
        movement = Vector2.zero;
    }

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
                   
                    _animator.SetTrigger(Attack);
                    _rigidbody2D.velocity = Vector2.zero;
                    attackCollider.gameObject.SetActive(true);
                    attackCounter = attackTimer;
                    
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

                if (isTutorial && TutorialManager.Manager.State == TutorialManager.TutorialState.MOVE)
                {
                    TutorialManager.Manager.HideTutorial();
                    TutorialManager.Manager.SetState(TutorialManager.TutorialState.JUMP);
                }

                movement = context.ReadValue<Vector2>();
                
                bool facingRight = _renderer.flipX;
                
                //change attack collider location
                bool changingDir = facingRight != movement.x <= 0;
                if (changingDir)
                {
                    var pos = attackCollider.gameObject.transform.localPosition;
                    pos.x *= -1;
                    attackCollider.gameObject.transform.localPosition = pos;
                }

                _renderer.flipX = movement.x <= 0;
                //_animator.SetBool(Walking, movement.x != 0);
                _animator.SetFloat(Action, WALKING);
                
                break;
            case InputActionPhase.Canceled:
                movement = Vector2.zero;
                //_animator.SetBool(Walking, false);
                _animator.SetFloat(Action, IDEAL);
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
                if (isTutorial && TutorialManager.Manager.State == TutorialManager.TutorialState.COLOR)
                {
                    TutorialManager.Manager.HideTutorial();
                    TutorialManager.Manager.SetState(TutorialManager.TutorialState.END);
                }
                ColorManager.RotateColor();
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }
    
    
}
