using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoom : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            Door door = other.gameObject.GetComponent<Door>();
            if (transform.position.x >= other.gameObject.transform.position.x)
            {
                door.EnterRightRoom();
            }
            else
            {
                door.EnterLeftRoom();
            }
        }
    }
}
