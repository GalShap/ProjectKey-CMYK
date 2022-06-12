using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QAJoke : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Kill()
    {
        print("kill");
        Destroy(gameObject,4);
    }
}
