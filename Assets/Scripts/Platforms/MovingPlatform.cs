using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    
    [Tooltip("The speed of the platform")]
    [SerializeField] private int speed = 1;
    
    [Tooltip("The index of the starting position of the platform")]
    [SerializeField] private int startPos = 0;
    
    [Tooltip("Points the platform has to move through")]
    [SerializeField] private List<Vector3> points;

    private int _curIndex = 0;

    private const float MIN_DISTANCE = 0.035f;

    private const int INITAL_POS_IDX = 0;

    private Rigidbody2D _rigidbody2D;

    private HashSet<Rigidbody2D> colliding = new HashSet<Rigidbody2D>();

    private Vector2 lastPos;

    public Vector3 nextLoc;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        lastPos = _rigidbody2D.position;
    }

    void Start()
    {
        transform.localPosition = points[startPos];
        _curIndex = startPos;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Rigidbody2D rb = other.collider.GetComponent<Rigidbody2D>();
        if(rb == null)
            return;
    
        if(!colliding.Contains(rb))
            colliding.Add(rb);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Rigidbody2D rb = other.collider.GetComponent<Rigidbody2D>();
        if(rb == null)
            return;
        
        if(colliding.Contains(rb))
            colliding.Remove(rb);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(points[_curIndex], transform.localPosition) < MIN_DISTANCE)
        {
            transform.localPosition = points[_curIndex];
            _curIndex++;
            if (_curIndex == points.Count)
            {
                _curIndex = INITAL_POS_IDX;
            }
        }
        
        _rigidbody2D.velocity = (points[_curIndex]-transform.localPosition).normalized * Time.fixedDeltaTime * speed;
        var delta = _rigidbody2D.position - lastPos;

        if (colliding.Count > 0)
        {
            colliding = new HashSet<Rigidbody2D>(colliding.Where(r => r != null).ToList());
            foreach (var rb in colliding)
            {
                rb.position += delta;
            }   
        }

        lastPos = _rigidbody2D.position;
    }

    public void Detach(Rigidbody2D rb)
    {
        if (colliding.Contains(rb))
        {
            colliding.Remove(rb);
        }
    }
}
