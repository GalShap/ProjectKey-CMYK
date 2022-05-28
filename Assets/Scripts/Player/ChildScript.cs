using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildScript : MonoBehaviour
{
    [SerializeField] private PlayerController parent;

    public void Attack()
    {
        print("attack");
        parent.StartAttack();
    }
}
