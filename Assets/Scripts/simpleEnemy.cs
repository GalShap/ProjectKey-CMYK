using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class simpleEnemy : EnemyObject
{
    [SerializeField] private Vector3 _currentTarget;
    [SerializeField] public GameObject player;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float radius = 4f;
    [SerializeField] private Transform[] places;

    [SerializeField] private int counter;

    // Start is called before the first frame update
    void Start()
    {
  
        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
            _renderer = GetComponentInChildren<SpriteRenderer>();
        
        collisionOffset = Vector2.right * (_renderer.sprite.rect.width / _renderer.sprite.pixelsPerUnit) / 2;
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
        if (!isAlive()) UponDead();
        isOnGround();
        // if (Math.Abs(PositionX()) <= len && Math.Abs(PositionY()) <= 0.3f && onGround)
        if (Vector3.Distance(transform.position, player.transform.position) < radius && onGround)
        {
            move();
        }
        else
        {
            moveByTarget();
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
        if (KickBackVector == null)
        {
            rb.velocity = new Vector2((PositionX() < 0 ? speed : -speed),
                rb.velocity.y);
        }

        else
        {
            rb.AddForce(KickBackVector.Value, ForceMode2D.Impulse);
            KickBackVector = null;
        }
    }

    /**
     * move the enemy by following the places transform positions.
     */
    void moveByTarget()
    {
        if (KickBackVector == null)
        {

            if (!onGround)
                counter = (counter >= places.Length - 1) ? 0 : counter + 1;

            var x = transform.position.x - places[counter].position.x;
            rb.velocity = new Vector2((x <= 0 ? speed : -speed),
                rb.velocity.y);
            if (Math.Abs(x) > 0.1f) return;
            counter = (counter >= places.Length - 1) ? 0 : counter + 1;
        }

        else
        {
            rb.AddForce(KickBackVector.Value, ForceMode2D.Impulse);
            KickBackVector = null;
        }
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