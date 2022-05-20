using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    #region Inspector

    [SerializeField] private int lives = 6;
    
    [SerializeField] private PlayerHUD playerHUD;
    
    [SerializeField] private float _bounce = 6f;
    
    #endregion
    
    #region Private Field
    
    private float _time = 0;

    private float _timeToBounce = 0.2f;
    
    private Rigidbody2D _playerRigidBody;
    
    private bool _isBouncing = false;
    
    #endregion
    
    #region Constants
  
    private const int MAX_LIVES = 6;

    private const int MIN_LIVES = 0;
   
    #endregion
    
    #region Mono Behaviour Funcs
    void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
        lives = PlayerHUD.MAX_LIFE;
    }

    // Update is called once per frame
    void Update()
    {
        // if (lives <= MIN_LIVES)
        // {
        //     Dead();
        // }
      
      
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

    void OnCollisionEnter2D(Collision2D other)
    {   
     
        if (EnemyCollision(other)) 
        {
            Damage(1);

            if (!_isBouncing)
            {
                _isBouncing = true;
                PlayerKickBack(other);
            }
            
        }
    }
    
    #endregion

    #region Private Methods    
    private bool EnemyCollision(Collision2D other)
    {
        return other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("Spikes"); //||
        //other.gameObject.CompareTag("Projectile");
    }
    
    /// <summary>
    ///  called when play is colliding with an enemy. kicks the player back by the "_bounce" parameter
    /// </summary>
    /// <param name="other">
    /// the collision with an enemy. 
    /// </param>
    private void PlayerKickBack(Collision2D other)
    {
        Rigidbody2D enemyRigidBody = other.gameObject.GetComponent<Rigidbody2D>();
        if (enemyRigidBody == null)
            enemyRigidBody = other.gameObject.GetComponentInParent<Rigidbody2D>();
        if (enemyRigidBody)
            return;
        _playerRigidBody.AddForce((_playerRigidBody.position - enemyRigidBody.position).normalized * _bounce, 
            ForceMode2D.Impulse);
        _isBouncing = false;

    }
    
    #endregion
    
    #region Public Methods

    /// <summary>
    ///  inflicts damage to player;
    /// </summary>
    /// <param name="amount">
    /// how many lives to remove! not how much health. 
    /// </param>
    public void Damage(int amount)
    {
        print($"Damage: {amount}");
        lives -= amount;
        if (lives <= MIN_LIVES)
        {
            lives = MIN_LIVES;
            Dead();
        }

        playerHUD.removeLifeOnUI(amount);

    }
    
    /// <summary>
    ///  heals player
    /// </summary>
    /// <param name="amount">
    /// how many lives to add! not how much health.
    /// </param>
    public void Heal(int amount)
    {
        print($"Heal: {amount}");
        lives += amount;
        if (lives >= MAX_LIVES)
            lives = MAX_LIVES;
        
        playerHUD.addLifeOnUI(amount);
    }

    public void SetHealth(int amount)
    {   
        if (amount == lives)
            return;

        if (amount < lives)
        {
            playerHUD.removeLifeOnUI(lives - amount);
        }

        else if (amount > lives)
        {
            playerHUD.addLifeOnUI(amount - lives);
        } 
        
        lives = amount;
        if (lives > MAX_LIVES)
            lives = MAX_LIVES;
        else if (lives < MIN_LIVES)
            lives = MIN_LIVES;

        
    }

    public int GetHealth()
    {
        return lives;
    }

    public void Dead()
    {
        SetHealth(MAX_LIVES);
        GameManager.Manager.Respawn();
    }
    
    #endregion
    
    
}
