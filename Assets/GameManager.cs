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
    
    [Range(0, 4)]
    [SerializeField] private int curMusicIndex;
    
    [SerializeField] private Whoosh whoosh;

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
        AudioManager.SharedAudioManager.PlayByIndex(curMusicIndex);
        currRoom.EnableContents();
    }

    #endregion

    #region Public Methods

    public void SetRespawn(GameObject respawn)
    {
        respawnPoint = respawn;
        respawnRoom = currRoom;
    }

    public void CreateWhoosh(Vector3 position)
    {
        Instantiate(whoosh, position, Quaternion.identity);
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
