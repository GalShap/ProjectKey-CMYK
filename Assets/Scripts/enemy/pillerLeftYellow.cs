using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillerLeftYellow : pillarScript
{
    [SerializeField] private GameObject blockEdge;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;

    // // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
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
        Vector2 target = new Vector2(left.transform.position.x, rb.transform.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        // rb.velocity = Vector2.left * speed;
    }

    public override void resetPostion()
    {
        base.resetPostion();
        blockEdge.SetActive(true);
    }

    protected override void MoveToMid()
    {
        Vector2 target = new Vector2(right.transform.position.x, rb.transform.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        // rb.velocity = Vector2.right * speed;
        // rb.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.fixedDeltaTime);
    }
}