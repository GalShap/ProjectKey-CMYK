using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /**
     * TODO:
     * 1. find best jump values
     * 2. smooth running : https://www.youtube.com/watch?v=USLp-4iwNnQ
     */
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
    
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _renderer;
    private Animator _animator;
    
    private bool onGround;
    private Vector2 movement;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        collisionOffset = Vector2.right * transform.lossyScale.x / 2;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float height = _renderer.sprite.rect.height / _renderer.sprite.pixelsPerUnit; 
        onGround = Physics2D.Raycast(
                       _rigidbody2D.position + collisionOffset,
                       Vector2.down,
                        height * 0.5f + 0.05f,
                       ColorManager.GroundLayers) 
                   || 
                   Physics2D.Raycast(
                       _rigidbody2D.position - collisionOffset,
                       Vector2.down,
                       height * 0.5f + 0.05f,
                       ColorManager.GroundLayers);
        
        MoveCharacter();
        if (jumpTimer > Time.time && onGround)
        {
            Jump();
        }
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
            
            _rigidbody2D.gravityScale = 0;
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

    public void onJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
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
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                movement = context.ReadValue<Vector2>();
                _renderer.flipX = movement.x <= 0;
                _animator.SetBool("Walking", movement.x != 0);
                break;
            case InputActionPhase.Canceled:
                movement = Vector2.zero;
                _animator.SetBool("Walking", false);
                break;
        }
    }

    public void OnChangeColor(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                ColorManager.RotateColor();
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }
    
    
}
