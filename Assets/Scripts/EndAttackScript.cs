using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAttackScript : MonoBehaviour
{
    private PlayerController parent;

    private void Start()
    {
        parent = GetComponentInParent<PlayerController>();
    }

    public void EndAttack()
    {
        parent.EndAttack();
    }
}
