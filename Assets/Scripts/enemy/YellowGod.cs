using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class YellowGod : EnemyObject
{
    [SerializeField]private GameObject[] leftPillars;
    [SerializeField]private GameObject leftBlocks;
    [SerializeField]private GameObject rightBlocks;
    [SerializeField]private GameObject downBlocks;
    [SerializeField]private GameObject upBlocks;
    [SerializeField]private GameObject[] rightPillars;
    [SerializeField]private GameObject[] upperPillars;
    [SerializeField]private GameObject[] groundPillars;
    [SerializeField] public GameObject b_Projectile; // this is a reference to your projectile prefab

    [SerializeField] public GameObject r_Projectile; // this is a reference to your projectile prefab

    [SerializeField]
    private Transform[] m_SpawnTransform; // this is a reference to the transform where the prefab will spawn

    [SerializeField] private int oddsForRed = 2;
    public GameObject player;
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
            leftPillars[i].gameObject.GetComponent<pillarScript>().resetPostion();
            rightPillars[i].gameObject.GetComponent<pillarScript>().resetPostion();
        }
        leftBlocks.SetActive(false);
        rightBlocks.SetActive(false);
    }public void HorizStart()
    {
        leftBlocks.SetActive(true);
        rightBlocks.SetActive(true);
    }
    public void PillarStart()
    {
        downBlocks.SetActive(true);
    }public void PillarEnd()
    {
        foreach (var t in groundPillars)
        {
            t.GetComponent<pillarScript>().resetPostion();
        }foreach (var t in upperPillars)
        {
            t.GetComponent<pillarScript>().resetPostion();
            t.SetActive(false);
        }
        downBlocks.SetActive(false);
        rb.position = new Vector2(0, rb.position.y);
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

    public void sendThemUp()
    {

        foreach (var pillar in groundPillars)
        {
            pillar.GetComponent<pillarScript>().GO();
            // ColorManager.ColorLayer
            // pillar.layer = Random.Range(ColorManager.ColorLayer)
                
            // pillar.GetComponent<ColorObject>().la
        }
    } public void sendThemDown()
    {
        int i = Random.Range(0, upperPillars.Length);
        for (int k = 0; k < upperPillars.Length; k++)
        {
            if (i != k)
            {
                upperPillars[k].SetActive(true);
                upperPillars[k].GetComponent<pillarScript>().GO();
            }
            else
            {
                rb.position = new Vector2(upperPillars[k].transform.position.x, rb.position.y);
            }
            
        }

        foreach (var pillar in upperPillars)
        {
            pillar.GetComponent<pillarScript>().GO();
            // ColorManager.ColorLayer
            // pillar.layer = Random.Range(ColorManager.ColorLayer)
                
            // pillar.GetComponent<ColorObject>().la
        }
    }
    
    
    public bool isFlipped = false;
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.transform.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.transform.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }


    public void healtChange()
    {
        var animator = gameObject.GetComponent<Animator>();
        var hl = animator.GetComponent<EnemyHealth>();
        var hp = hl.GetHealth();
        if (hp <= 400)
        {
            animator.SetBool("rage", true);
        }
    }
}
