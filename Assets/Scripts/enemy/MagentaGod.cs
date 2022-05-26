using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagentaGod : EnemyObject
{
    [SerializeField] private float timerCounter = 3;
    [SerializeField] public float speed = 3;
    [SerializeField] public float attackRange = 3;
    private float timer = 0;
    public GameObject m_Projectile; // this is a reference to your projectile prefab
    public Transform m_SpawnTransform; // this is a reference to the transform where the prefab will spawn
    public GameObject player;
    
    // Start is called before the first frame update
    private void Awake()
    {
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        collisionOffset = Vector2.right * (_renderer.sprite.rect.width / _renderer.sprite.pixelsPerUnit) / 2;

    }

    // // Update is called once per frame
    // void Update()
    // {
    //     timer -= Time.deltaTime;
    //     if (!(timer <= 0)) return;
    //     Shoot();
    //     timer = timerCounter;
    // }

    public void Shoot()
    {
        Instantiate(m_Projectile, m_SpawnTransform.position, m_SpawnTransform.rotation);
    }

    protected override void UponDead()
    {
        gameObject.SetActive(false);
    }
    
    public bool isFlipped = false;

    public void LookAtPlayer()
    {
        // Vector3 flipped = transform.localScale;
        // flipped.z *= -1f;
        //
        // if (transform.position.x > player.transform.position.x && isFlipped)
        // {
        //     transform.localScale = flipped;
        //     transform.Rotate(0f, 180f, 0f);
        //     isFlipped = false;
        // }
        // else if (transform.position.x < player.transform.position.x && !isFlipped)
        // {
        //     transform.localScale = flipped;
        //     transform.Rotate(0f, 180f, 0f);
        //     isFlipped = true;
        // }
    }
}
