using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillarBarriar : MonoBehaviour
{
    [SerializeField] private pillarScript.PilerSide Side;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("pillar") )
        {
            pillarScript pil = other.GetComponent<pillarScript>();
            if(pil.getSide() == Side)
                pil.CollideWithOtherBlock();
        }
    }
}
