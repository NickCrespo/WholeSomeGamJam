using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CultGuardSM : MonoBehaviour
{
    public Transform[] navPoints;
    public float idleTimer;
    public float detectionTimer = 1.5f;
    public float distractionTimer;
    public float detectionRange;

    public bool repositioning = false;
    Transform Target;

    Transform player;
    GameObject[] slaves;

    //[HideInInspector]
    public int navIndex = 0;
    NavMeshAgent enemy;


    public enum States
    {
        Idle, // Base state of doing nothing
        Patrol, // Moving to navpoints
        Investigate, // Question mark upon spotting player
        Distract,
        Chase
    }

    public States states;

    [HideInInspector] public States currState = States.Patrol;

    Dictionary<States, Action> SM = new Dictionary<States, Action>();

    private void Awake()
    {
        SM.Add(States.Idle, new Action(StateIdle));
        SM.Add(States.Patrol, new Action(StatePatrol));
        SM.Add(States.Investigate, new Action(StateInvestigate));
        SM.Add(States.Distract, new Action(StateDistract));
        SM.Add(States.Chase, new Action(StateChase));

        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        slaves = GameObject.FindGameObjectsWithTag("Slave");
    }

    public void Update()
    {
        SM[currState]?.Invoke();
    }

    public void SetState(States newState)
    {
        currState = newState;
    }

    public virtual void StateIdle()
    {
        //Start timer
        idleTimer -= Time.deltaTime;
        Debug.Log("Timer running");

        if (idleTimer <= 0)
        {
            Debug.Log("Entering Patrol");
            SetState(States.Patrol);
            idleTimer = 1;
        }
        else
        {
            Debug.Log("Timer running");
            enemy.isStopped = true;
        }

        foreach (GameObject slave in slaves)
        {
            float DistanceToSlave = Vector3.Distance(transform.position, slave.transform.position);
            if (DistanceToSlave <= detectionRange)
            {
                //detectedSlave = true;
                SetState(States.Distract);
            }
        }

        float DistanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (DistanceToPlayer <= detectionRange)
        {
            //detectedPlayer = true;
            SetState(States.Investigate);
        }
    }

    public virtual void StatePatrol()
    {
        Debug.Log("Entered Patrol");
        enemy.SetDestination(navPoints[navIndex].transform.position);
        repositioning = true;

        foreach (GameObject slave in slaves)
        {
            float DistanceToSlave = Vector3.Distance(transform.position, slave.transform.position);
            if (DistanceToSlave <= detectionRange)
            {
                Target = slave.transform;
                //repositioning = false;
                Debug.Log("Entering Distract");
                SetState(States.Distract);
            }
        }

        float DistanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (DistanceToPlayer <= detectionRange)
        {
            repositioning = false;
            Debug.Log("Entering Investigate");
            SetState(States.Investigate);
        }
    }

    public virtual void StateInvestigate()
    {
        enemy.isStopped = true;
        // enable the UI question mark indicator
        detectionTimer -= Time.deltaTime;

        float DistanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (detectionTimer <= 0 && DistanceToPlayer <= detectionRange)
        {
            Debug.Log("Entering Chase");
            SetState(States.Chase);
        }
        else
        {
            Debug.Log("Entering Patrol");
            SetState(States.Patrol);
        }
    }

    public virtual void StateDistract()
    {
        Debug.Log("Entered Distract");
        // Enable exclamation point UI
        float DistanceToTarget = Vector3.Distance(transform.position, Target.position);
        enemy.SetDestination(Target.position);
        if (DistanceToTarget <= enemy.stoppingDistance)
        {
            distractionTimer -= Time.deltaTime;
            if (distractionTimer <= 0)
            {
                // Kill slave
                Target = null;
                SetState(States.Patrol);
            }
        }
        else
        {
            distractionTimer = 3f;
        }
    }

    public virtual void StateChase()
    {
        Debug.Log("Entered Chase");
        enemy.SetDestination(player.position);
        // Level failure on collision
    }
}
