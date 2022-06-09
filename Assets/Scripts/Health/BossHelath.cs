using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHelath : EnemyHealth
{
   private Animator anim;
   protected override void Start()
   {
      anim = gameObject.GetComponent<Animator>();
      base.Start();
   }

   public override void Hit(GameObject hitter)
   {
      anim.SetTrigger("hit");
      base.Hit(hitter);
   }

   public override void Dead()
   {
      anim.SetTrigger("coolDown");  
      base.Dead();
   }

   public void CoolDown()
   {
      anim.SetTrigger("coolDown");
   }
}
