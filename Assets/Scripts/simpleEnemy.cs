using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class simpleEnemy : enemyObject
{
    [SerializeField] private Vector3 _currentTarget;
    [SerializeField] public GameObject player;

    [SerializeField] private float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _currentTarget = player.transform.position;
    }

    void FixedUpdate()
    {
        if (!isAlive())
        {
            gameObject.SetActive(false);
        }
        move();
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