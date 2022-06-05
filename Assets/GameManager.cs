using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region Inspector

    [SerializeField] private PlayerController playerController;
    [SerializeField] private Room currRoom;

    #endregion

    #region Fields

    public static GameManager Manager;
    private Room respawnRoom;
    private GameObject respawnPoint;

    #endregion

    #region MonoBehavior

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
        AudioManager.SharedAudioManager.Play();
        currRoom.EnableContents();
    }

    #endregion

    #region Public Methods

    public void SetRespawn(GameObject respawn)
    {
        respawnPoint = respawn;
        respawnRoom = currRoom;
    }

    public void SetRoom(Room r)
    {
        currRoom = r;
        CameraManager.Manager.Camera = r.Camera;
    }

    public void Respawn()
    {
        CameraManager.Manager.Camera = respawnRoom.Camera;
        currRoom = respawnRoom;
        
        playerController.transform.position = respawnPoint.transform.position;
    }

    #endregion
}
