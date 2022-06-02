using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class YellowGod : EnemyObject
{
    [SerializeField]private GameObject[] leftPillars;
    [SerializeField]private GameObject[] rightPillars;
    [SerializeField]private GameObject[] upperPillars;
    [SerializeField]private GameObject[] groundPillars;
    [SerializeField]
    private Transform[] m_SpawnTransform; // this is a reference to the transform where the prefab will spawn
    [SerializeField] public GameObject _Projectile; // this is a reference to your projectile prefab

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
            leftPillars[i].gameObject.SetActive(false);
            rightPillars[i].gameObject.SetActive(false);
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
        int i = m_SpawnTransform.Length;
        for (int k = 0; k < m_SpawnTransform.Length; k++)
        {
            Instantiate(_Projectile, m_SpawnTransform[k].position, m_SpawnTransform[k].rotation);
        }
    }
    
    // public void 
    
    
    
    
    
}
