using UnityEngine;
using System;

public class Timer
{
    bool on;
    float timeStart;
    float timeLength;
    float timeElapsed;
    bool repeat = false;
    string id;
    bool reset;
    public event Action action;

    public string ID
    {
        get
        {
            return id;
        }
    }
    public bool IsOn
    {
        get
        {
            return on;
        }
    }
    public float TimeStart
    {
        get
        {
            return timeStart;
        }
    }
    public float TimeLength
    {
        get
        {
            return timeLength;
        }
    }
    public float TimeElapsed
    {
        get
        {
            return timeElapsed;
        }
        set
        {
            timeElapsed = value;
        }
    }
    public float TimeRemaining
    {
        get
        {
            return timeLength - timeElapsed;
        }
    }
    public bool IsRepeating
    {
        get
        {
            return repeat;
        }
        set
        {
            repeat = value;
        }
    }
    public bool IsReset
    {
        get
        {
            return reset;
        }
    }


    public Timer(bool start = true, float _time = 0, Action _action = null, bool _repeat = true, string _id = "")
    {
        Reset();

        if (start)
            Start();
        else
            Reset();

        timeLength = _time;

        if (_action != null)
            action += _action;

        repeat = _repeat;

        id = _id;
    }

    public void Start()
    {
        on = true;

        timeStart = Time.time;

        timeElapsed = 0;

        reset = false;
    }
    public void Start(float _time)
    {
        on = true;

        timeStart = Time.time;

        timeLength = _time;

        timeElapsed = 0;

        reset = false;
    }
    public void StartRandom(float minTime, float maxTime)
    {
        on = true;

        timeStart = Time.time;

        timeLength = UnityEngine.Random.Range(minTime, maxTime);

        timeElapsed = 0;

        reset = false;

    }
    public void SetID(string _id)
    {
        id = _id;
    }
    public bool Run()
    {
        if (on)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeLength)
            {
                InvokeAction();

                if (repeat)
                    Start(timeLength);
                else
                    Stop();
                return true;
            }
        }
        return false;
    }
    public void Stop()
    {
        on = false;
    }
    public void Resume()
    {
        on = true;
    }
    public void Restart()
    {
        on = true;
        timeElapsed = 0;
    }
    public void Reset()
    {
        on = false;

        timeStart = 0;

        timeLength = 0;

        timeElapsed = 0;

        reset = true;
    }
    public void AddAction(Action _action)
    {
        action += _action;
    }
    public void InvokeAction()
    {
        if (action != null)
            action.Invoke();
    }
}
