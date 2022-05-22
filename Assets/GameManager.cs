using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private Room currRoom;
    private Room respawnRoom;
    private GameObject respawnPoint;

    private void Awake()
    {
        if (Manager == null)
        {
            Manager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currRoom.Camera.Priority = 1;
        currRoom.EnableContents();
    }

    public void SetRespawn(GameObject respawn)
    {
        respawnPoint = respawn;
        respawnRoom = currRoom;
    }

    public void SetRoom(Room r)
    {
        currRoom = r;
    }

    public void Respawn()
    {
        currRoom.Camera.Priority--;
        respawnRoom.Camera.Priority++;
        
        playerController.transform.position = respawnPoint.transform.position;
    }
}
