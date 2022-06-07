using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChild : MonoBehaviour
{
    public void Dead()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
