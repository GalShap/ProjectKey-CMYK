using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 100;

    private const int MAX_HEALTH = 100;

    private const int MIN_HEALTH = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackCollider"))
        {
            print("damage");
            Damage(50);
        }
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (health < MIN_HEALTH)
            health = MIN_HEALTH;
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > MAX_HEALTH)
            health = MAX_HEALTH;
    }

    public void SetHealth(int amount)
    {
        health = amount;
        if (health > MAX_HEALTH)
            health = MAX_HEALTH;
        
        else if (health < MIN_HEALTH)
            health = MIN_HEALTH;
    }

    public int GetHealth()
    {
        return health;
    }

    public void Dead()
    {
        Debug.Log("Enemy is Dead!");
    }
}
