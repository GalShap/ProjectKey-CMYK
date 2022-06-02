using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yellowPillarRight : pillarScript
{
    // // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }
    
    public override void Back()
    {
        moveForwed = false;
        moveBack = true;
    }

    public override void StopMovement()
    {
        moveForwed = false;
        moveBack = false;
    }

    public override void GO()
    {
        moveForwed = true;
        moveBack = false;
    }

    public override void CollideWithOtherBlock()
    {
        StopMovement();
    }
}
