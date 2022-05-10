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
    }

    
    // physics is best, when activating it in Fixed update. 
    private void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        if (Math.Abs(PositionX()) <= len && Math.Abs(PositionY()) <= 0.4f)
        {
            move();
        }
    }

    protected override void UponDead()
    {
        gameObject.SetActive(false);
    }


    /**
     * move the enemy by following the player.
     */
    void move()
    {
        rb.velocity = new Vector2((PositionX() < 0 ? speed : -speed),
            rb.velocity.y);
    }
    
    
    private float PositionX()
    {
        return (transform.position.x - player.transform.position.x);
    }

    private float PositionY()
    {
        return (transform.position.y - player.transform.position.y);
    }
}
