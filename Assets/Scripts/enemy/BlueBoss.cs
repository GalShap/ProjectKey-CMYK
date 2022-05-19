using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBoss : EnemyObject
{
    [SerializeField] private float timerCounter = 3;
    private float timer = 0;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float jumpTime = 1f;
    private float jumpTimer;
    private float dir = 1;
    private bool startFight = false;
    [SerializeField] private float jumpDelay = 0.25f;
    [SerializeField] private float jumpLength = 2f;
    
    
    private void Awake()
    {
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        collisionOffset = Vector2.right * (_renderer.sprite.rect.width/_renderer.sprite.pixelsPerUnit) / 2;
        dir = dir * jumpLength;
    }

    // Update is called once per frame
    void Update()
    {
        jumpTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (jumpTimer > timerCounter)
        {
            Jump();
        }
    }

    protected override void UponDead()
    {
        gameObject.SetActive(false);
    }
    private void Jump()
    {
        rb.drag = 0;
        var y = (2 * jumpHeight) / jumpTime;
        rb.velocity = new Vector2(dir, y);
        jumpTimer = 0;
        dir *= -1;
    }
    
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("AttackCollider"))
    //     {
    //         DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.BLUE_DEAD, true, () =>
    //         {
    //             TimelineManager.Manager.Play();
    //         });
    //     }
    // }
}
