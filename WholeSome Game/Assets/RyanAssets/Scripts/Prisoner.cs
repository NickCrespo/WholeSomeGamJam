using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Prisoner : StateMachine, IInteractable
{
    NavMeshAgent navAgent;
    Vector3 targetPosition;
    internal Player player;
    internal bool focused = false;
    [SerializeField] internal Sense sense;

    public float DistanceToTarget
    {
        get
        {
            return Vector3.Distance(transform.position, targetPosition);
        }
    }
    public bool AtTarget
    {
        get
        {
            return DistanceToTarget / 2 < navAgent.stoppingDistance;
        }
    }
    public float PersonalSpace
    {
        get
        {
            return navAgent.stoppingDistance * 4;
        }
    }
    public bool Focused
    {
        get
        {
            return focused;
        }
    }

    public override void Setup()
    {
        base.Setup();

        navAgent = GetComponent<NavMeshAgent>();
        targetPosition = transform.position;

        player = FindObjectOfType<Player>();
    }
    public override void AddStates()
    {
        AddState(typeof(PrisonerStateIdle));
        AddState(typeof(PrisonerStateFollow));
        AddState(typeof(PrisonerStateDistract));

        SetInitialState(typeof(PrisonerStateIdle));
    }
    public void GoIdle() // player command
    {
        focused = false;

        ChangeState(typeof(PrisonerStateIdle));
    }
    public void FollowPlayer() // player command
    {
        ChangeState(typeof(PrisonerStateFollow));
    }
    public void Distract(Vector3 newPosition) // player command
    {
        NavToPosition(newPosition);

        focused = false;

        ChangeState(typeof(PrisonerStateDistract));
    }
    public bool Follow()
    {
        return NavToPosition(player.FollowPosition);
    }
    public void Interact()
    {
        if (IsCurrentState(typeof(PrisonerStateIdle)))
        {
            player.AddFollower(this);
            // change color to indicate focused
        }
        else if (IsCurrentState(typeof(PrisonerStateFollow)))
        {
            // change color to indicate focused
        }

        focused = true;
    }
    public bool NavToPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
        navAgent.isStopped = false;
        return navAgent.SetDestination(targetPosition);
    }
    public void StopNav()
    {
        navAgent.isStopped = true;
    }
}
