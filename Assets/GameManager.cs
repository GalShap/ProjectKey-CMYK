using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private Room currRoom;
    
    private float shakeCounter;
    private Vector3 look;
    
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
        CameraManager.Manager.Camera = currRoom.Camera;
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
        CameraManager.Manager.Camera = respawnRoom.Camera;
        currRoom = respawnRoom;
        
        playerController.transform.position = respawnPoint.transform.position;
    }
}
