
using System.Collections.Generic;
using UnityEngine;

public class MovingObject: MonoBehaviour
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

    // Update is called once per frame
    protected  void FixedUpdate()
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
        lastPos = _rigidbody2D.position;
    }
}