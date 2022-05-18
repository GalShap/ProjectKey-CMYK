using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : EnemyObject
{
    [SerializeField] public GameObject player;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected Transform[] places;
    [SerializeField] protected int counter;
    // Start is called before the first frame update

    private void Awake()
    {
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        counter = 0;
        _renderer = GetComponentInChildren<SpriteRenderer>();
        collisionOffset = Vector2.right * (_renderer.sprite.rect.width/_renderer.sprite.pixelsPerUnit) / 2;

    }
    
    // physics is best, when activating it in Fixed update. 
    private void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        isOnGround();
        Move();
    }

   
    /**
     * move the enemy by following the places transform positions.
     */
    protected void Move()
    {
        var x = transform.position.x - places[counter].position.x;
        rb.velocity = new Vector2((x <= 0 ? speed : -speed),
            rb.velocity.y);
        if (Math.Abs(x) > 0.1f) return;
        counter = (counter >= places.Length - 1) ? 0 : counter + 1;
    }
    
    protected override void UponDead()
    {
        gameObject.SetActive(false);
    }
    
}