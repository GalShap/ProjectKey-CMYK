using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    #region Inspector

    [SerializeField] private int lives = 6;
    
    [SerializeField] private PlayerHUD playerHUD;

    #endregion

    #region Constants
  
    private int MAX_LIVES = 6;

    private int MIN_LIVES = 0;
   
    #endregion
   
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
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {   
        // todo review it with alon and harel!
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Projectile"))
            Damage(1);
        
        // todo should an animation be played?
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
        Debug.Log("Damage! Cur Health is: " + lives);
        
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
        Debug.Log("Key is Dead!");
    }
    
    
}
