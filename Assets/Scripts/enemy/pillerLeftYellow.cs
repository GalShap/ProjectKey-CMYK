using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillerLeftYellow : pillarScript
{
    [SerializeField] private GameObject blockEdge;
    
    // // Start is called before the first frame update
    void Start()
    {
        side = PilerSide.LEFT;
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
        blockEdge.SetActive(false);
        StopMovement();
    }
    protected override void MoveBack()
    {
        rb.velocity = Vector2.left * speed;
        // rb.position = Vector2.MoveTowards(transform.position, pointA.position, speed * Time.fixedDeltaTime);
    }
    
    public override void resetPostion()
    {
        base.resetPostion();
        blockEdge.SetActive(true);
    }

    protected override void MoveToMid()
    {
        rb.velocity = Vector2.right * speed;
        // rb.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.fixedDeltaTime);
    }
}
