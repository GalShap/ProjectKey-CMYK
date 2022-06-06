using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yellowPillarRight : pillarScript
{
    // // Start is called before the first frame update
    void Start()
    {
        side = PilerSide.RIGHT;
    }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }

   

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
        rb.velocity = Vector2.right * speed;
        // rb.position = Vector2.MoveTowards(transform.position, pointA.position, speed * Time.fixedDeltaTime);
    }

    protected override void MoveToMid()
    {
        rb.velocity = Vector2.left * speed;
        // rb.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.fixedDeltaTime);
    }
}
