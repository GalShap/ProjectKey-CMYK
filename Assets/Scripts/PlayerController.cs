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
    [Header("Physics")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float linearDrag = 5f;
    

    [SerializeField] private float maxSped = 7;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float fallMultiplier = 4f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private LayerMask groundLayers;
    
    private Rigidbody2D _rigidbody2D;
    private bool onGround;
    private bool jumping;
    private Vector2 movement;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        onGround = Physics2D.Raycast(
            transform.position, 
            Vector2.down, 
            transform.lossyScale.y * 0.55f, 
            ColorManager.GroundLayers);
    }

    private void FixedUpdate()
    {
        SetVelocity();
        ModifyPhysics();
    }

    private void SetVelocity()
    {
        Vector2 vel = _rigidbody2D.velocity;
        vel.x = movement.x * moveSpeed;
        _rigidbody2D.AddForce(vel);
        // _rigidbody2D.velocity = vel;
        SetGravity();
    }

    private void ModifyPhysics()
    {
        bool changingDirection = 0 > movement.x * _rigidbody2D.velocity.x;
        if (Math.Abs(_rigidbody2D.velocity.x) < 0.4 || changingDirection)
        {
            _rigidbody2D.drag = linearDrag;
        }
        else
        {
            _rigidbody2D.drag = 0f;
        }
    }
    private void SetGravity()
    {
        float scale = 0;
        if (onGround)
        {
            scale = 0;
        }
        else
        {
            scale = gravityScale;
            if (_rigidbody2D.velocity.y < 0)
            {
                scale *= fallMultiplier;
            }
            else if (_rigidbody2D.velocity.y > 0 && !jumping)
            {
                scale *= fallMultiplier / 2;
            }
        }

        _rigidbody2D.gravityScale = scale;
    }

    public void onJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if(!onGround)
                    return;
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x,0);
                _rigidbody2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
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
