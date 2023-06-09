using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class YellowGod : EnemyObject
{
    [SerializeField] private GameObject[] leftPillars;
    [SerializeField] private GameObject leftBlocks;
    [SerializeField] private GameObject rightBlocks;
    [SerializeField] private GameObject downBlocks;
    [SerializeField] private GameObject upBlocks;
    [SerializeField] private GameObject[] rightPillars;
    [SerializeField] private GameObject[] upperPillars;
    [SerializeField] private GameObject[] groundPillars;
    [SerializeField] public GameObject b_Projectile; // this is a reference to your projectile prefab

    [SerializeField] public GameObject r_Projectile; // this is a reference to your projectile prefab

    [SerializeField]
    private Transform[] m_SpawnTransform; // this is a reference to the transform where the prefab will spawn
    

    [SerializeField] private int oddsForRed = 2;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private ColorOrb orb;
    [SerializeField] private GameObject otherSprite;
    public GameObject player;
    public GameObject leftBarrier;
    public GameObject rightBarrier;
    private Vector3 initPlace;

    private void Awake()
    {
        initPlace = transform.position;
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        collisionOffset = Vector2.right * (_renderer.sprite.rect.width / _renderer.sprite.pixelsPerUnit) / 2;
    }

    public void Die()
    {
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.YELLOW_DEAD_1, true,
            () =>
            {   
                otherSprite.gameObject.SetActive(false);
                gameObject.SetActive(false);
                orb.gameObject.SetActive(true);
            });
    }

    public void HorizEnd()
    {
        for (int i = 0; i < leftPillars.Length; i++)
        {
            // if (leftPillars[i].gameObject.activeSelf)
            {
                leftPillars[i].gameObject.GetComponent<pillarScript>().resetPostion();
                leftPillars[i].gameObject.GetComponent<ColorObject>().ChangeColor(ColorManager.ColorName.Yellow);
            }

            // if (rightPillars[i].gameObject.activeSelf)
            {
                rightPillars[i].gameObject.GetComponent<pillarScript>().resetPostion();
                rightPillars[i].gameObject.GetComponent<ColorObject>().ChangeColor(ColorManager.ColorName.Yellow);
            }
        }

        leftBlocks.SetActive(false);
        rightBlocks.SetActive(false);

        if (rb.transform.position != initPlace)
        {
            ColorManager.ToggleYellow(
                (() => {sprite.gameObject.SetActive(false);}),
                (() => {
                    rb.transform.position = initPlace;
                    sprite.gameObject.SetActive(true);}));   
        }
    }

    public void HorizStart()
    {
        leftBlocks.SetActive(true);
        rightBlocks.SetActive(true);
    }

    public void PillarStart()
    {
        downBlocks.SetActive(true);
        foreach (var t in upperPillars)
        {
            // t.GetComponent<pillarScript>().resetPostion();
            t.SetActive(true);
        }
    }

    public void PillarEnd()
    {
        foreach (var t in groundPillars)
        {
            if(t.activeSelf)
                t.GetComponent<pillarScript>().resetPostion();
        }

        foreach (var t in upperPillars)
        {
            if (t.activeSelf)
            {
                t.GetComponent<pillarScript>().resetPostion();
                t.SetActive(false);
            }
                
        }

        downBlocks.SetActive(false);
        // downBlocks.

        if (rb.transform.position != initPlace)
        {
            ColorManager.ToggleYellow(
                (() => {sprite.gameObject.SetActive(false);}),
                (() => {
                    rb.transform.position = initPlace;
                    sprite.gameObject.SetActive(true);}));   
        }
        // rb.position = new Vector2(upperPillars[0].GetComponent<pillarScript>().transform.position.x, rb.position.y);
    }


    protected override void UponDead()
    {
        throw new System.NotImplementedException();
    }

    public void HorizMoveBlock(int i)
    {
        var animator = gameObject.GetComponent<Animator>();
        if (i < leftPillars.Length && i < rightPillars.Length)
        {
            leftPillars[i].gameObject.GetComponent<pillarScript>().GO();
            rightPillars[i].gameObject.GetComponent<pillarScript>().GO();
            animator.SetTrigger("attack");
        }

        animator.SetTrigger("stopAttack");
    }

    public void horizMoveBack(int i)
    {
        var animator = gameObject.GetComponent<Animator>();
        if (i < leftPillars.Length && i < rightPillars.Length)
        {
            leftPillars[i].gameObject.GetComponent<pillarScript>().Back();
            rightPillars[i].gameObject.GetComponent<pillarScript>().Back();
        }
    }

    public void ChangeColorOfBlock(int i)
    {
        // ColorManager.ColorName color_ = Random.Range(0,1) == 0? ColorManager.ColorName.Cyan: ColorManager.ColorName.Magenta;
        leftPillars[i].gameObject.GetComponent<ColorObject>().ChangeColor((int) Random.Range(0, 2) == 1
            ? ColorManager.ColorName.Cyan
            : ColorManager.ColorName.Magenta);
        rightPillars[i].gameObject.GetComponent<ColorObject>().ChangeColor((int) Random.Range(0, 2) == 1
            ? ColorManager.ColorName.Cyan
            : ColorManager.ColorName.Magenta);
    }

    public void HorizMoveBack()
    {
        
        for (var i = 0; i < leftPillars.Length; i++)
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
        var animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("attack");
        foreach (var pillar in groundPillars)
        {
            pillar.gameObject.GetComponent<ColorObject>().ChangeColor((int) Random.Range(0, 2) == 1
                ? ColorManager.ColorName.Cyan
                : ColorManager.ColorName.Magenta);
            pillar.GetComponent<pillarScript>().GO();
        }

        animator.SetTrigger("stopAttack");
    }

    public void sendThemDown()
    {
        var animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("attack");
        int i = Random.Range(0, upperPillars.Length);
        for (int k = 0; k < upperPillars.Length; k++)
        {
            if (i != k)
            {
                // upperPillars[k].SetActive(true);
                upperPillars[k].GetComponent<pillarScript>().GO();
                // print("go");
            }
            else
            {
                int t = k;
                ColorManager.ToggleYellow(
                    (() => {sprite.gameObject.SetActive(false);}),
                    (() => {
                        rb.transform.position = new Vector2(upperPillars[t].transform.position.x, rb.position.y);
                        sprite.gameObject.SetActive(true);}));
            }
        }

        animator.SetTrigger("stopAttack");

        // foreach (var pillar in upperPillars)
        // {
        //     pillar.GetComponent<pillarScript>().GO();
        //     // ColorManager.ColorLayer
        //     // pillar.layer = Random.Range(ColorManager.ColorLayer)
        //         
        //     // pillar.GetComponent<ColorObject>().la
        // }
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

    public void resetBoss()
    {
        var animator = gameObject.GetComponent<Animator>();
        var hl = animator.GetComponent<EnemyHealth>();
        hl.SetHealth(1000);
        animator.SetTrigger("reset");
    }

    public void stopBoss()
    {
        var animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("stop");
    }

    public void resumeBoss()
    {
        var animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("resume");
    }

    public void resetBossActive()
    {
        var animator = gameObject.GetComponent<Animator>();
        if (player.GetComponent<PlayerHealth>().GetHealth() <= 0)
        {
            resetBoss();
            animator.SetBool("rage", false);
            animator.SetTrigger("stop");
        }
    }
}