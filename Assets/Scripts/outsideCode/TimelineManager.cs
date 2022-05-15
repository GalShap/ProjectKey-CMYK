using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private List<PlayableDirector> timelines;
    
    private int currIndex = 0;
    private PlayableDirector _currTimeline;
    public static TimelineManager Manager;
    public bool IsPlaying => Manager._currTimeline.state == PlayState.Playing;

    private void Awake()
    {
        if (Manager == null)
        {
            Manager = this;
            _currTimeline = timelines[0];
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play()
    {
        _currTimeline.Play();
    }

    public void Pause()
    {
        _currTimeline.Pause();
    }

    public void NextTimeline()
    {
        currIndex++;
        if (currIndex < timelines.Count)
            _currTimeline = timelines[currIndex];
    }
}
