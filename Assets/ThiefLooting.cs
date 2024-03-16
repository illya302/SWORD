using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThiefLooting : StateMachineBehaviour
{
    private float chasingSpeed;
    private float scaredDistandce;

    private GameObject thief;
    private ThiefLogic thiefLogic;
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        thief = animator.gameObject;
        thiefLogic = thief.GetComponent<ThiefLogic>();
        player = GameObject.Find("Hero");
        navMeshAgent = thief.GetComponent<NavMeshAgent>();
        scaredDistandce = thiefLogic.scaredDistandce;
        chasingSpeed = thiefLogic.chasingSpeed;

        navMeshAgent.speed = chasingSpeed;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(thief.transform.position, player.transform.position) < scaredDistandce)
        {
            navMeshAgent.ResetPath();
            animator.SetBool("RunAway", true);
        }
        else if (thiefLogic.lootTarget != null)
        {
            navMeshAgent.SetDestination(thiefLogic.lootTarget.transform.position);
        }
        else if ((thiefLogic.lootTarget == null))
        {
            navMeshAgent.ResetPath();
            animator.SetBool("Looting",false);
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
