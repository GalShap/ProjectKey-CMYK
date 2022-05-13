using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteMovment : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<SpriteRenderer>().flipX = gameObject.GetComponent<Rigidbody2D>().velocity.x < 0;
    }
}
