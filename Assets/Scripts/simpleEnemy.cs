using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class simpleEnemy : enemyObject
{
    [SerializeField] private Vector3 _currentTarget;
    [SerializeField] public GameObject player;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float len = 4f;
    [SerializeField] private Transform[] places;

    [SerializeField] private int counter;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        counter = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        _currentTarget = player.transform.position;
    }

    // physics is best, when activating it in Fixed update. 
    private void FixedUpdate()
    {
        if (!isAlive()) gameObject.SetActive(false);
        if (Math.Abs(PositionX()) <= len && Math.Abs(PositionY()) <= 0.3f)
        {
            move();
        }
        else
        {
            moveByTarget();
        }
    }

    /**
     * move the enemy by following the player.
     */
    void move()
    {
        rb.velocity = new Vector2((PositionX() < 0 ? speed : -speed),
            rb.velocity.y);
    }

    /**
     * move the enemy by following the places transform positions.
     */
    void moveByTarget()
    {
        var x = transform.position.x - places[counter].position.x;
        rb.velocity = new Vector2((x <= 0 ? speed : -speed),
            rb.velocity.y);
        if (Math.Abs(x) > 0.1f) return;
        counter = (counter >= places.Length - 1) ? 0 : counter + 1;
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