using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    #region Inspector

    [SerializeField] private int health = 100;
    
    [SerializeField] private SpriteRenderer enemySpriteRenderer;

    [Tooltip("How much time the animation takes for every blip?")] 
    [SerializeField] private float timeToAnimate = 0.1f;
    
    #endregion

    #region Constants
    
    private const int MAX_HEALTH = 100;

    private const int MIN_HEALTH = 0;

    private const int MAX_ALPHA = 1;

    private const int MIN_ALPHA = 0;
    
    #endregion

    private IEnumerator DamageFlashAnimation(int count)
    {   
        
        float elapsedTimeMax = 0;
        float elapsedTimeMin = 0;
        Color alpha = enemySpriteRenderer.color;
        for (int i = 0; i < count; i++)
        {
            elapsedTimeMax = 0;
            elapsedTimeMin = 0;
            
            while (elapsedTimeMin < timeToAnimate)
            {   
      
                alpha.a =  Mathf.Lerp(MAX_ALPHA, MIN_ALPHA, (elapsedTimeMin / timeToAnimate));
                enemySpriteRenderer.color = alpha;
                elapsedTimeMin += Time.deltaTime;
                yield return null;
            }

            alpha.a = MIN_ALPHA;
            
            while (elapsedTimeMax < timeToAnimate)
            {   
         
                alpha.a =  Mathf.Lerp(MIN_ALPHA, MAX_ALPHA, (elapsedTimeMax / timeToAnimate));
                enemySpriteRenderer.color = alpha;
                elapsedTimeMax += Time.deltaTime;
                yield return null;
            }

            alpha.a = MAX_ALPHA;
        }
        
        
    }
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
        StartCoroutine(DamageFlashAnimation(1));
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
