using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillarScript : MonoBehaviour
{
    protected Rigidbody2D rb;
    private bool moveForwed;
    private bool moveBack;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 3;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (moveForwed)
        {
            rb.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.fixedDeltaTime);
        }
        else if (moveBack)
        {
            rb.position = Vector2.MoveTowards(transform.position, pointA.position, speed * Time.fixedDeltaTime);
        }
    }

    public void go()
    {
        moveForwed = true;
        moveBack = false;
    }
    public void back()
    {
        moveForwed = false;
        moveBack = true;
    }
}