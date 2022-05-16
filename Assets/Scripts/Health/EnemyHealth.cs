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

    [SerializeField] private float bounce = 6f;

    [SerializeField] private float _timeToBounce = 0.2f;
    
    #endregion

    private Rigidbody2D _enemyRigidBody;

    private float _time = 0f;

    private bool _isBouncing = false;

    #region Constants
    
    private const int MAX_HEALTH = 100;

    private const int MIN_HEALTH = 0;

    private const int MAX_ALPHA = 1;

    private const int MIN_ALPHA = 0;
    
    #endregion

    private void Start()
    {
        _enemyRigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isBouncing)
        {
            _time += Time.deltaTime;
            if (_time >= _timeToBounce)
            {
                _isBouncing = false;
                _time = 0;
            }
            
        }
    }

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
            Damage(50);
            
            if (!_isBouncing)
            {
                _isBouncing = true;
                EnemyKickBack(other);
            }
        }
    }

    private void EnemyKickBack(Collider2D other)
    {
        Rigidbody2D _playerRigidBody = other.gameObject.GetComponent<Rigidbody2D>();
        if (_playerRigidBody == null)
            _playerRigidBody = other.gameObject.GetComponentInParent<Rigidbody2D>();
        _playerRigidBody.AddForce((_enemyRigidBody.position - _playerRigidBody.position).normalized * bounce,
            ForceMode2D.Impulse);
        _isBouncing = false;
    }


    public void Damage(int amount)
    {
        StartCoroutine(DamageFlashAnimation(1));
        health -= amount;
        if (health < MIN_HEALTH)
        {
            health = MIN_HEALTH;
            Dead();
        }
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
        gameObject.SetActive(false);
    }
}
