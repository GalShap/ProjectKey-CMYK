using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Room leftRoom;
    [SerializeField] private Room rightRoom;

    private void SetPriorities(int left, int right)
    {
        // dont add to priority if already positive
        if (leftRoom.Camera.Priority * left > 0 || rightRoom.Camera.Priority * right > 0)
        {
            print("here");
            return;
        }
        leftRoom.Camera.Priority += left;
        rightRoom.Camera.Priority += right;
    }

    public void SetLeftCamera() => SetPriorities(1,-1);
    public void SetRightCamera() => SetPriorities(-1,1);


}
