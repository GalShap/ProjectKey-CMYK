using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowGod : EnemyObject
{
    [SerializeField]private GameObject[] leftPillars;
    [SerializeField]private GameObject[] rightPillars;
    [SerializeField]private GameObject[] upperPillars;
    [SerializeField]private GameObject[] groundPillars;
    
    
    private void Awake()
    {
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        collisionOffset = Vector2.right * (_renderer.sprite.rect.width / _renderer.sprite.pixelsPerUnit) / 2;

    }

    // // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }
    

    protected override void UponDead()
    {
        throw new System.NotImplementedException();
    }
    
    
}
