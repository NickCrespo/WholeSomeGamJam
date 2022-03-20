using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected Dictionary<Type, State> states = new Dictionary<Type, State>();
    protected State initialState;
    protected State currentState;
    protected State nextState;
    protected bool onEnter;
    protected bool onExit;

    public virtual void Awake()
    {
        Setup();
    }
    public virtual void Update()
    {
        if (onExit)
        {
            currentState.StateExit();
            currentState = nextState;
            nextState = null;

            onEnter = true;
            onExit = false;
        }

        if (onEnter)
        {
            currentState.StateEnter();

            onEnter = false;
        }

        currentState.StateUpdate();
    }
    public virtual void FixedUpdate()
    {
        if (!onEnter && !onExit)
        {
            currentState.StateFixedUpdate();
        }
    }
    public virtual void LateUpdate()
    {
        if (!onEnter && !onExit)
        {
            currentState.StateLateUpdate();
        }
    }
    public virtual void Setup()
    {
        AddStates();

        currentState = initialState;

        foreach (var keyValuePair in states)
        {
            keyValuePair.Value.StateSetup();
        }

        onEnter = true;
        onExit = false;
    }
    public abstract void AddStates();
    public void SetInitialState(Type T)
    {
        initialState = states[T];
    }
    public void ChangeState(Type T)
    {
        nextState = states[T];
        onExit = true;
    }
    public bool IsCurrentState(Type T)
    {
        if (currentState.GetType() == T)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void AddState(Type T)
    {
        if (!states.ContainsKey(T))
        {
            State s = (State)Activator.CreateInstance(T);
            s.machine = this;

            states.Add(T, s);
        }
    }
}
