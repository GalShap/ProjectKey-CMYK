using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHelath : EnemyHealth
{
   // MaxHealth = 600;
   // public override void Dead()
   // {
   //    Animator anim = gameObject.GetComponent<Animator>();
   //    anim.SetTrigger("Death");
   //    // base.Dead();
   //    
   // }

   public override void Hit(GameObject hitter)
   {
      
      Animator anim = gameObject.GetComponent<Animator>();
      base.Hit(hitter);
      anim.SetTrigger("hit");
   }
}
