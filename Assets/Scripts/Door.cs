using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera leftCam;
    [SerializeField] private CinemachineVirtualCamera rightCam;

    private void SetPriorities(int left, int right)
    {
        // dont add to priority if already positive
        if (leftCam.Priority * left > 0 || rightCam.Priority * right > 0)
        {
            print("here");
            return;
        }
        leftCam.Priority += left;
        rightCam.Priority += right;
    }

    public void SetLeftCamera() => SetPriorities(1,-1);
    public void SetRightCamera() => SetPriorities(-1,1);


}
