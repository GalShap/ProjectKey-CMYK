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
        }
        else
        {
            EnableDialogue();
        }
    }

    private void EnableDialogue()
    {
        print("enable dialogue");
        _playerMap.Disable();
        _dialogueMap.Enable();
        
        print(input.actions.FindActionMap("Dialogue").enabled);
    }
    
    private void EnablePlayer()
    {
        _playerMap.Enable();
        _dialogueMap.Disable();
    }
}
