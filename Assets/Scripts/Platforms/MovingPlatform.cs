using System.Collections;
using System.Collections.Generic;
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

    private const float MIN_DISTANCE = 0.02f;

    private const int INITAL_POS_IDX = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = points[startPos];
        _curIndex = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(points[_curIndex], transform.localPosition) < MIN_DISTANCE)
        {
            _curIndex++;
            if (_curIndex == points.Count)
            {
                _curIndex = INITAL_POS_IDX;
            }
        }

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, points[_curIndex],
            speed * Time.deltaTime);
    }
}
