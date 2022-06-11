using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upPillar : pillarScript
{
    protected override void Awake()
    {
        base.Awake();
        side = PilerSide.UP;
    }
    
    public override void StopMovement()
    {
        moveForwed = false;
        moveBack = false;
    }


    public override void CollideWithOtherBlock()
    {
        StopMovement();
    }

    protected override void MoveBack()
    {
        rb.velocity = Vector2.up * speed;
        // rb.position = Vector2.MoveTowards(transform.position, pointA.position, speed * Time.fixedDeltaTime);
    }

    protected override void MoveToMid()
    {
        rb.velocity = Vector2.down * speed;
        // rb.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.fixedDeltaTime);
    }
    
}
