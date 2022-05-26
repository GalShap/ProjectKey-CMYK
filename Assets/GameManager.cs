using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private Room currRoom;

    [Header("Camera Shake")]
    [SerializeField] private float shakeAmplitude;
    [SerializeField] private float shakeTime;
    private float shakeCounter;
    
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

    private void Update()
    {
        if (shakeCounter > 0)
        {
            shakeCounter -= Time.deltaTime;
        }
        else
        {
            StopShake();
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

    public void ShakeCamera()
    {
        var perlin = currRoom.Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = shakeAmplitude;
        shakeCounter = shakeTime;
    }
    
    private void StopShake()
    {
        var perlin = currRoom.Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 0;
    }
}
