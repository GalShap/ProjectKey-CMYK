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

    private float _time = 0;

    private float _timeToBounce = 0.2f;
    
    private Rigidbody2D _playerRigidBody;
    
    private bool _isBouncing = false;
    
    #region Constants
  
    private int MAX_LIVES = 6;

    private int MIN_LIVES = 0;
   
    #endregion

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
        lives = PlayerHUD.MAX_LIFE;
    }

    // Update is called once per frame
    void Update()
    {
        if (lives == MIN_LIVES)
        {
            Dead();
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
            Damage(1);
        
        else if (Input.GetKeyDown(KeyCode.X))
            Heal(1);

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

    private void OnCollisionEnter2D(Collision2D other)
    {   
        // todo review it with alon and harel!
        if (other.gameObject.CompareTag("Monster") ||  false)//other.gameObject.CompareTag("Projectile"))
        {
            Damage(1);

            if (other.gameObject.CompareTag("Monster") && !_isBouncing)
            {
                _isBouncing = true;
                PlayerKickBack(other);
            }
            
        }
    }
    
    /// <summary>
    ///  called when play is colliding with an enemy. kicks the player back by the "_bounce" parameter
    /// </summary>
    /// <param name="other">
    /// the collision with an enemy. 
    /// </param>
    private void PlayerKickBack(Collision2D other)
    {   
        Debug.Log("kick back!");
        Rigidbody2D enemyRigidBody = other.gameObject.GetComponent<Rigidbody2D>();
        _playerRigidBody.AddForce((_playerRigidBody.position - enemyRigidBody.position).normalized * _bounce, 
            ForceMode2D.Impulse);
        _isBouncing = false;

    }
  

    /// <summary>
    ///  inflicts damage to player;
    /// </summary>
    /// <param name="amount">
    /// how many lives to remove! not how much health. 
    /// </param>
    public void Damage(int amount)
    {
        lives -= amount;
        if (lives < MIN_LIVES)
            lives = MIN_LIVES;
        
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
        lives += amount;
        if (lives > MAX_LIVES)
            lives = MAX_LIVES;
        
        playerHUD.addLifeOnUI(amount);
        Debug.Log("Heal! Cur Health is: " + lives);
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
        
    }
    
    
}
