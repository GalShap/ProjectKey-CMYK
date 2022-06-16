using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PatrolEnemy : EnemyObject
{
    [FormerlySerializedAs("player")] [SerializeField] public GameObject _player;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected Transform[] places;

    [SerializeField] protected int counter;
    [SerializeField] protected bool isStatic = false;

    private void Awake()
    {

        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        counter = 0;
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
            _animator = GetComponent<Animator>();
        collisionOffset = Vector2.right * (_renderer.sprite.rect.width / _renderer.sprite.pixelsPerUnit) / 2;
    }

    // physics is best, when activating it in Fixed update. 
    private void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        isOnGround();
        Move();
    }


    /**
     * move the enemy by following the places transform positions.
     */
    protected void Move()
    {
        if (KickBackVector == null)
        {
            if (places.Length <= 1 || places[0] == null || isStatic)
            {
                rb.velocity = Vector3.Slerp(rb.velocity, Vector3.zero, 1);
                return;
            }
            var x = transform.position.x - places[counter].position.x;
                    rb.velocity = new Vector2((x <= 0 ? speed : -speed),
                        rb.velocity.y);
            if (Math.Abs(x) > 0.1f) return;
            counter = (counter >= places.Length - 1) ? 0 : counter + 1;
            
        }

        else
        {
            // rb.AddForce(KickBackVector.Value, ForceMode2D.Impulse);
            rb.velocity = KickBackVector.Value;
            KickBackVector = null;
        }
       
    }

    public override void OnColorChange(ColorManager.ColorLayer layer)
    {
        base.OnColorChange(layer);
        var health = GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.damagable = gameObject.layer == ColorManager.NeutralIndex || !colored;
        }
    }

    protected override void UponDead()
    {
        gameObject.SetActive(false);
    }
}