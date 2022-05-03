using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class simpleEnemy : enemyObject
{
    [SerializeField] public Vector3 _currentTarget;
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
        // _currentTarget = player.transform.position;
        move();
        // rb.velocity = Vector3.MoveTowards(player.transform.position, transform.position, speed);
        // rb.AddForce(-1 * Vector3.MoveTowards(transform.position, _currentTarget, speed * Time.deltaTime));
    }

    void move()
    {
        float x = transform.position.x - player.transform.position.x;
        float y = transform.position.y - player.transform.position.y;
        movement = transform.position;
        if (x < 0)
        {
            movement.x = transform.position.x + 1;
        }
        else
        {
            movement.x = transform.position.x -1;
        }

        // if (y < 0)
        // {
        //     movement.y = transform.position.y +1;
        // }
        // else
        // {
        //     movement.y = transform.position.y -1;
        // }
        // Vector3 x = transform.position - player.transform.position;
        rb.MovePosition(Vector3.MoveTowards(transform.position, movement, speed * Time.deltaTime));
        // if(player.transform.position.x) 
    }
    
}
