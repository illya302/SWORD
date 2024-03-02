using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class SpikeIdle : StateMachineBehaviour
{
    private float travelTimeMax;
    private float travelDistance;
    private float chaseDistance;
    private float travelTime;
    private float defaultSpeed;
    private GameObject spike;
    private SpikeLogic spikeLogic;
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spike = animator.gameObject;
        player = GameObject.Find("Hero");
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        spikeLogic = animator.GetComponent<SpikeLogic>();

        travelTimeMax = spikeLogic.travelTimeMax;
        travelDistance = spikeLogic.travelDistance;
        chaseDistance = spikeLogic.chaseDistance;
        defaultSpeed = spikeLogic.defaultSpeed;

        navMeshAgent.speed = defaultSpeed;

        Vector3 destination = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
        navMeshAgent.SetDestination(spike.transform.position + (destination * travelDistance));
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        travelTime -= Time.deltaTime;
        if (travelTime < 0) 
        { 
            travelTime = travelTimeMax;
            Vector3 destination = new Vector3 (Random.Range(-1.0f,1.0f), Random.Range(-1.0f, 1.0f), 0);
            navMeshAgent.SetDestination(spike.transform.position + (destination*travelDistance));
        }
        if (player != null && Vector3.Distance(player.transform.position, spike.transform.position) < chaseDistance) 
        {
            navMeshAgent.ResetPath();
            animator.SetBool("PlayerClose", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
