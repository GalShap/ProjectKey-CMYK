using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteMovment : MonoBehaviour
{
    private SpriteRenderer ren;
    private Rigidbody2D rig;

    // Start is called before the first frame update
    void Start()
    {
        ren = gameObject.GetComponentInChildren<SpriteRenderer>();
        rig = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ren.flipX = rig.velocity.x < 0;
    }
}