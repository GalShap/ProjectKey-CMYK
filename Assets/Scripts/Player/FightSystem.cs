using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSystem : MonoBehaviour
{
    [SerializeField] private PlayerHUD hud; 
    private Rigidbody2D _rigidbody;
    private int lives;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        lives = PlayerHUD.MaxLife;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Monster"))
        {
            Vector2 normal = other.contacts[0].normal;
            _rigidbody.AddForce(normal);

            if (lives > 0)
            {
                lives -= 1;
                hud.removeLifeOnUI(1);
            }
        }
    }
}
