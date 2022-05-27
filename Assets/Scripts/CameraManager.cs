using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Manager;
    
    [Header("Camera Shake")]
    [SerializeField] private float shakeAmplitude;
    [SerializeField] private float shakeTime;
    
    [Header("Look Ahead")]
    [SerializeField] private float lookSpeed;
    [SerializeField] private float lookDistance;

    private CinemachineVirtualCamera _currCamera;
    private CinemachineTransposer _transposer;
    private CinemachineBasicMultiChannelPerlin _perlin;
    
    private float shakeCounter;
    private Vector3 lookAhead;

    public CinemachineVirtualCamera Camera
    {
        get => _currCamera;
        set
        {
            if (_currCamera != null)
            {
                _currCamera.Priority--;
            }
            _currCamera = value;
            _currCamera.Priority++;
            _transposer = _currCamera.GetCinemachineComponent<CinemachineTransposer>();
            _perlin = _currCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

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

        MoveCamera();
    }

    private void MoveCamera()
    {
        if (_transposer.m_FollowOffset == lookAhead) return;
        
        _transposer.m_FollowOffset = Vector3.Slerp(_transposer.m_FollowOffset, lookAhead, lookSpeed * Time.deltaTime);
        if (Vector3.Distance(_transposer.m_FollowOffset, lookAhead) < 0.05)
            _transposer.m_FollowOffset = lookAhead;
    }
    
    public void ShakeCamera()
    {
        _perlin.m_AmplitudeGain = shakeAmplitude;
        shakeCounter = shakeTime;
    }
    
    private void StopShake()
    {
        _perlin.m_AmplitudeGain = 0;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                lookAhead = context.ReadValue<Vector2>();
                break;
            case InputActionPhase.Canceled:
                lookAhead = Vector2.zero;
                break;
        }
    }
}