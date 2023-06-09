using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField] private Room leftRoom;
    [SerializeField] private Room rightRoom;
    
    [SerializeField] private UnityEvent onEnterLeft;

    [SerializeField] private UnityEvent onEnterRight;
    
    private void SetPriorities(int left, int right)
    {
        if (leftRoom.Camera.Priority * left > 0 || rightRoom.Camera.Priority * right > 0)
        {
            return;
        }
        leftRoom.Camera.Priority += left;
        rightRoom.Camera.Priority += right;
    }

    public void EnterLeftRoom()
    {
        GameManager.Manager.SetRoom(leftRoom);
        leftRoom.EnableContents();
        rightRoom.DisableContents();
        EnterLeft();
        // SetLeftCamera();
    }
    
    public void EnterRightRoom()
    {
        GameManager.Manager.SetRoom(rightRoom);
        rightRoom.EnableContents();
        leftRoom.DisableContents();
        EnterRight();
        
        // SetRightCamera();
    }

    public void SetLeftCamera() => SetPriorities(1,-1);
    public void SetRightCamera() => SetPriorities(-1,1);

    public void EnterLeft()
    {   
        if (onEnterLeft != null) onEnterLeft.Invoke();
    }

    public void EnterRight()
    {
        if (onEnterRight != null) onEnterRight.Invoke();
    }
}
