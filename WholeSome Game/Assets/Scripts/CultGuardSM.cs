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
    Transform Target;

    Transform player;
    GameObject[] slaves;

    [HideInInspector]
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

    public virtual void Update()
    {
        SM[currState]?.Invoke();
    }

    public void SetState(States newState)
    {
        currState = newState;
    }

    void StateIdle()
    {
        Debug.Log("Entered Idle");
        //Start timer
        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0)
        {
            SetState(States.Patrol);
        }
        else
        {
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

    void StatePatrol()
    {
        Debug.Log("Entered Patrol");
        enemy.SetDestination(navPoints[navIndex].transform.position);

        foreach (GameObject slave in slaves)
        {
            float DistanceToSlave = Vector3.Distance(transform.position, slave.transform.position);
            if (DistanceToSlave <= detectionRange)
            {
                Target = slave.transform;
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

    void StateInvestigate()
    {
        Debug.Log("Entered Investigate");
        enemy.isStopped = true;
        // enable the UI question mark indicator
        detectionTimer -= Time.deltaTime;

        float DistanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (detectionTimer <= 0 && DistanceToPlayer <= detectionRange)
        {
            SetState(States.Chase);
        }
        else
        {
            SetState(States.Patrol);
        }
    }

    void StateDistract()
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

    void StateChase()
    {
        Debug.Log("Entered Chase");
        enemy.SetDestination(player.position);
        // Level failure on collision
    }
}
