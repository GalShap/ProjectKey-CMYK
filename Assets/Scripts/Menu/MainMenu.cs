using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour
{

    [SerializeField] private GameObject playArrow;

    [SerializeField] private GameObject exitArrow;

    [SerializeField] private AudioSource audioSource;
    
    private int _curMode = 0;
    
    public enum Mode
    {
        Play, Exit
    }

    private void Awake()
    {
        playArrow.SetActive(true);
        exitArrow.SetActive(false);
    }

    private IEnumerator wait( float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            var val = context.ReadValue<Vector2>();
            if (val.y > 0)
                MoveUp();
            else if (val.y < 0)
                MoveDown();   
        }
    }
    private void MoveDown()
    {   
        audioSource.Play();
        if (_curMode == (int) Mode.Play)
        {
            _curMode = (int) Mode.Exit;
            playArrow.SetActive(false);
            exitArrow.SetActive(true);

        }
        
        else if (_curMode == (int) Mode.Exit)
        {
            _curMode = (int) Mode.Play;
            playArrow.SetActive(true);
            exitArrow.SetActive(false);
        }
    }

    private void MoveUp()
    {   
        audioSource.Play();
        if (_curMode == (int) Mode.Exit)
        {
            _curMode = (int) Mode.Play;
            playArrow.SetActive(true);
            exitArrow.SetActive(false);
        }
        
        else if (_curMode == (int) Mode.Play)
        {
            _curMode = (int) Mode.Exit;
            playArrow.SetActive(false);
            exitArrow.SetActive(true);
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            switch (_curMode)
            {
                case (int) Mode.Play:
                    LoadGame();
                    break;
            
                case (int) Mode.Exit:
                    Exit();
                    break;
            }   
        }
    }
    
    public void LoadGame()
    {
        StartCoroutine(wait(0.2f));
        SceneManager.LoadScene("Tutorial scene");
    }

    public void Exit()
    {
        StartCoroutine(wait(0.2f));
        Application.Quit();
    }
    
}
