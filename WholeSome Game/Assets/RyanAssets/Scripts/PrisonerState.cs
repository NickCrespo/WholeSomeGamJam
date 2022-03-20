using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerState : State
{
    internal Prisoner prisoner;

    internal override StateMachine machine
    {
        get
        {
            return prisoner;
        }
        set
        {
            prisoner = (Prisoner)value;
        }
    }
}

public class PrisonerStateIdle : PrisonerState
{
    public override void StateEnter()
    {

    }
    public override void StateUpdate()
    {
        
    }
    public override void StateExit()
    {

    }
}

public class PrisonerStateFollow : PrisonerState
{
    public override void StateEnter()
    {

    }
    public override void StateUpdate()
    {
        if (prisoner.DistanceToTarget < prisoner.PersonalSpace * 2 && !prisoner.player.IsMoving && prisoner.sense.TooCrowdedInFront)
        {
            prisoner.StopNav();
        }
        else
        {
            prisoner.Follow();
        }
    }
    public override void StateExit()
    {

    }
}

public class PrisonerStateDistract : PrisonerState
{
    public override void StateEnter()
    {

    }
    public override void StateUpdate()
    {
        if (prisoner.AtTarget)
        {
            prisoner.GoIdle();
        }
    }
    public override void StateExit()
    {

    }
}