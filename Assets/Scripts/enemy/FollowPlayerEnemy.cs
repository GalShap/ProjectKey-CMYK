using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerEnemy : EnemyObject
    { 
    [SerializeField] private Vector3 _currentTarget;
    [SerializeField] public GameObject player;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float len = 4f;

    [SerializeField] private int counter;
    
    private void Awake()
    {
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        counter = 0;
        _renderer = GetComponentInChildren<SpriteRenderer>();
        collisionOffset = Vector2.right * (_renderer.sprite.rect.width/_renderer.sprite.pixelsPerUnit) / 2;

    }

    
    // physics is best, when activating it in Fixed update. 
    protected void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        isOnGround();
        if(Math.Abs(PositionY()) < len)
            Move();
        
    }
    
    protected override void UponDead()
    {
        gameObject.SetActive(false);
    }


    /**
     * move the enemy by following the player.
     */
    protected void Move()
    {
        float i =onGround ? 1f : -1f;
        rb.velocity = new Vector2((PositionX() < 0 ? speed : -speed) * i,
            rb.velocity.y);
    }
    
    
    private float PositionX()
    {
        return (transform.position.x - player.GetComponent<Transform>().position.x);
    }

    private float PositionY()
    {
        return (transform.position.y - player.GetComponent<Transform>().position.y);
    }
}
