using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    
    public static InputManager Manager;
    private InputActionMap _playerMap;
    private InputActionMap _dialogueMap;
    private bool playerWasLast = true;

    private void Awake()
    {
        if (Manager == null)
        {
            Manager = this;
            _playerMap = input.actions.FindActionMap("Player");
            _dialogueMap = input.actions.FindActionMap("Dialogue");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleMaps(bool enablePlayer)
    {
        if (enablePlayer)
        {
            EnablePlayer();
            playerWasLast = true;
        }
        else
        {
            EnableDialogue();
            playerWasLast = false;
        }
    }

    private void EnableDialogue()
    {
        _playerMap.Disable();
        _dialogueMap.Enable();
    }
    
    private void EnablePlayer()
    {
        _playerMap.Enable();
        _dialogueMap.Disable();
    }

    public void DisableAll()
    {
        _playerMap.Disable();
        _dialogueMap.Disable();
    }

    public void EnableAll()
    {
        _playerMap.Enable();
        _dialogueMap.Enable();
    }
}
