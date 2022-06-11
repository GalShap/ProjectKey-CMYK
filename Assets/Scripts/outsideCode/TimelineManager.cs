using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private List<PlayableDirector> timelines;
    
    [SerializeField] private List<UnityEvent> timelineEvents;
    
    private int currIndex = 1;
    private PlayableDirector _currTimeline;
    public static TimelineManager Manager;
    public int startAt = 0;
    private int index = 0;
    public bool IsPlaying => Manager._currTimeline != null && Manager._currTimeline.state == PlayState.Playing;

    private void Awake()
    {
        if (Manager == null)
        {
            Manager = this;
            _currTimeline = timelines[currIndex];
            index = startAt;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void NextEvent()
    {
        timelineEvents[index].Invoke();
        index++;
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
        {
            _currTimeline = timelines[currIndex];
            _currTimeline.Play();
        }
    }
}
