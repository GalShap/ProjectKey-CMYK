using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
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
    public float scale;
    private float jumpTimer;
    private bool jumping;
    
    [Header("Collision")]
    [SerializeField] private LayerMask groundLayers;
    private Vector3 collisionOffset;
    
    private Rigidbody2D _rigidbody2D;
    private bool onGround;
    private Vector2 movement;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        collisionOffset = Vector3.right * transform.lossyScale.x / 2;
    }

    private void Update()
    {
        onGround = Physics2D.Raycast(
            transform.position + collisionOffset,
            Vector2.down,
            transform.lossyScale.y * 0.55f,
            ColorManager.GroundLayers) 
                   || 
                   Physics2D.Raycast(
            transform.position - collisionOffset,
            Vector2.down,
            transform.lossyScale.y * 0.55f,
            ColorManager.GroundLayers);
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        if (jumpTimer > Time.time && onGround)
        {
            Jump();
        }
        ModifyPhysics();
        scale = _rigidbody2D.gravityScale;
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
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x,0);
        print(_rigidbody2D.mass);
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
                break;
            case InputActionPhase.Canceled:
                movement = Vector2.zero;
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
