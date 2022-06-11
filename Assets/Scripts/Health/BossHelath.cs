using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHelath : EnemyHealth
{
   private Animator anim;
   public bool isYellow = false;
   public SpriteRenderer sprite;
   protected override void Start()
   {
      anim = gameObject.GetComponent<Animator>();
      if (anim == null)
         anim = gameObject.GetComponentInChildren<Animator>();
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
      if (isYellow)
      {
         ColorManager.ToggleYellow(
            (()=>{sprite.gameObject.SetActive(false);}),
            (() => {base.Dead();}));
      }
   }

   public void CoolDown()
   {
      anim.SetTrigger("coolDown");
   }
}
