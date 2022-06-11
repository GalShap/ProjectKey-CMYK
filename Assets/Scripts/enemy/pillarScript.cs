using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class pillarScript : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected bool moveForwed;
    protected bool moveBack;
    protected Vector3 startPostion;

    // [SerializeField] protected GameObject OtherBlock;
    // [SerializeField] protected GameObject blockToStop;

    // [SerializeField] private Transform pointA;
    // [SerializeField] private Transform pointB;
    [SerializeField] protected float speed = 6;
    protected PilerSide side;
    private bool isSet = false;

    public enum PilerSide
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    };

    protected virtual void Awake()
    {
        if (!isSet)
        {
            print("one");
            rb = gameObject.GetComponent<Rigidbody2D>();
            startPostion = transform.position;
            isSet = true;
        }
        else
        {
            print("somthing");
        }
        print("two");
    }

    
    protected void FixedUpdate()
    {
        if (moveForwed)
        {
            MoveToMid();
        }
        else if (moveBack)
        {
            MoveBack();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    protected abstract void MoveBack();
    // {
    // rb.velocity = Vector2.down * speed;
    // rb.position = Vector2.MoveTowards(transform.position, pointA.position, speed * Time.fixedDeltaTime);
    // }

    protected abstract void MoveToMid();
    // {
    // rb.velocity = Vector2.up * speed;
    // rb.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.fixedDeltaTime);
    // }

    //set the block to move 
    public void GO()
    {
        moveForwed = true;
        moveBack = false;
    }

    // set the block to move back
    public void Back()
    {
        moveForwed = false;
        moveBack = true;
    }

    // set the block to stop moving
    public abstract void StopMovement();

    public abstract void CollideWithOtherBlock();

    // public void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject == OtherBlock)
    //     {
    //         CollideWithOtherBlock();
    //     }
    // }

    public virtual void resetPostion()
    {
        rb.transform.position = startPostion;
    }

    public PilerSide getSide()
    {
        return side;
    }
}