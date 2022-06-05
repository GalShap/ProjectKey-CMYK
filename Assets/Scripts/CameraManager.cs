using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Manager;
    
    [Header("Camera Shake")]
    [SerializeField] private float shakeAmplitude = 5;
    [SerializeField] private float shakeTime = 0.2f;
    
    [Header("Look Ahead")]
    [SerializeField] private float lookSpeed = 1;
    [SerializeField] private float lookDistance = 1;

    //[SerializeField] private Camera mainCamera; 

    private CinemachineVirtualCamera _currCamera;
    private CinemachineFramingTransposer _transposer;
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
            _transposer = _currCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
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
        if (_transposer.m_TrackedObjectOffset == lookAhead) return;
        
        _transposer.m_TrackedObjectOffset = Vector3.Slerp(_transposer.m_TrackedObjectOffset, lookAhead, lookSpeed * Time.deltaTime);
        if (Vector3.Distance(_transposer.m_TrackedObjectOffset, lookAhead) < 0.05)
            _transposer.m_TrackedObjectOffset = lookAhead;
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
                lookAhead = Vector2.up * (context.ReadValue<Vector2>().y * lookDistance);
                break;
            case InputActionPhase.Canceled:
                lookAhead = Vector2.zero;
                break;
        }
    }

    public bool CanCameraSee(Collider2D obj)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(UnityEngine.Camera.main);
        Bounds colliderBounds = obj.bounds;
        colliderBounds.Expand(8f);
        return GeometryUtility.TestPlanesAABB(planes , colliderBounds);
    }
}