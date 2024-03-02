using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpikeChasing : StateMachineBehaviour
{
    private GameObject spike;
    private SpikeLogic spikeLogic;
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    private float chaseDistance;
    private float attackDistance;
    private float chasingSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spike = animator.gameObject;
        player = GameObject.Find("Hero");
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        spikeLogic = animator.GetComponent<SpikeLogic>();

        chaseDistance = spikeLogic.chaseDistance;
        attackDistance = spikeLogic.attackDistance;
        chasingSpeed = spikeLogic.chasingSpeed;

        navMeshAgent.speed = chasingSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(player.transform.position, animator.transform.position);
        if (distance > attackDistance && distance < chaseDistance)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        else if (distance < attackDistance && distance < chaseDistance)
        {
            navMeshAgent.ResetPath();
            animator.SetBool("Attack", true);
        }
        else if (distance > chaseDistance) 
        {
            navMeshAgent.ResetPath();
            animator.SetBool("PlayerClose", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
