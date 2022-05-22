using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using UnityEngine;

public class BackgroundMachine : MonoBehaviour
{
    [SerializeField] private GameObject machine;
    [SerializeField] private SpriteRenderer currBG;
    [SerializeField] private SpriteRenderer nextBG;
    [SerializeField] private float rotateSpeed = 0.2f;
    private Action onRotate;

    private IEnumerator RotateBackground(float time)
    {
        float start = machine.transform.eulerAngles.z;
        float end = start - 180 - (start % 180);
        float elapsedTime = 0;
        float x = machine.transform.rotation.eulerAngles.x;
        float y = machine.transform.rotation.eulerAngles.y;
        
        float z;
        Vector3 rotation = new Vector3(0,0,0);

        while (elapsedTime < time)
        {
            z = Mathf.Lerp(start, end, elapsedTime / time);
            rotation = new Vector3(x, y, z);
            machine.transform.rotation = Quaternion.Euler(rotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rotation.z = end;
        machine.transform.rotation = Quaternion.Euler(rotation);

        if (onRotate != null)
        {
            onRotate.Invoke();
            onRotate = null;
        }

        (nextBG, currBG) = (currBG, nextBG);
    }

    public void Rotate(Color c, [CanBeNull] Action onRotate)
    {
        this.onRotate = onRotate;
        nextBG.color = c;
        StartCoroutine(RotateBackground( rotateSpeed));   
    }
}
