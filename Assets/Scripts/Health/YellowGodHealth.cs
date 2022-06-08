using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowGodHealth : EnemyHealth
{
    // MaxHealth = 600;
    public override void Dead()
    {
        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("dead");
        // base.Dead();
      
    }

    public override void Hit(GameObject hitter)
    {
      
        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("hit");
        base.Hit(hitter);
    }
}
