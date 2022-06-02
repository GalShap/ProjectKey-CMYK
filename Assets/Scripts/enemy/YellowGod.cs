using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class YellowGod : EnemyObject
{
    [SerializeField]private GameObject[] leftPillars;
    [SerializeField]private GameObject[] rightPillars;
    [SerializeField]private GameObject[] upperPillars;
    [SerializeField]private GameObject[] groundPillars;
    [SerializeField] public GameObject b_Projectile; // this is a reference to your projectile prefab

    [SerializeField] public GameObject r_Projectile; // this is a reference to your projectile prefab

    [SerializeField]
    private Transform[] m_SpawnTransform; // this is a reference to the transform where the prefab will spawn

    [SerializeField] private int oddsForRed = 2;
    private void Awake()
    {
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        collisionOffset = Vector2.right * (_renderer.sprite.rect.width / _renderer.sprite.pixelsPerUnit) / 2;

    }

    // // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    public void HorizEnd()
    {
        for (int i = 0; i < leftPillars.Length; i++)
        {
            leftPillars[i].gameObject.SetActive(false);
            rightPillars[i].gameObject.SetActive(false);
        }
    }public void HorizStart()
    {
        for (int i = 0; i < leftPillars.Length; i++)
        {
            leftPillars[i].gameObject.SetActive(true);
            rightPillars[i].gameObject.SetActive(true);
        }
    }
    
    
    protected override void UponDead()
    {
        throw new System.NotImplementedException();
    }

    public void HorizMoveBlock(int i)
    {
        if ( i < leftPillars.Length && i < rightPillars.Length)
        {
            leftPillars[i].gameObject.GetComponent<pillarScript>().GO();
            rightPillars[i].gameObject.GetComponent<pillarScript>().GO();
        }
    }

    public void HorizMoveBack()
    {
        for (int i = 0; i < leftPillars.Length; i++)
        {
            leftPillars[i].gameObject.GetComponent<pillarScript>().Back();
            rightPillars[i].gameObject.GetComponent<pillarScript>().Back();
        }
    }

    public GameObject[] getLeftPil()
    {
        return leftPillars;
    } 
    public GameObject[] getrightPil()
    {
        return rightPillars;
    }
    
    public void Shoot()
    {
        var proj = Random.Range(0, oddsForRed) == 1 ? r_Projectile : b_Projectile;
        foreach (var t in m_SpawnTransform)
        {
            Instantiate(proj, t.position, t.rotation);
        }
    }
    
    // public void 
    
    
    
    
    
}
