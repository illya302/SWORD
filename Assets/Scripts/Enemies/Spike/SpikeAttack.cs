using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpikeAttack : StateMachineBehaviour
{
    private GameObject spike;
    private SpikeLogic spikeLogic;
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    private float attackDistance;
    private float scaredDistance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spike = animator.gameObject;
        player = GameObject.Find("Hero");
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        spikeLogic = animator.GetComponent<SpikeLogic>();

        attackDistance = spikeLogic.attackDistance;
        scaredDistance = spikeLogic.scaredDistandce;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
            return;
        
        float distance = Vector3.Distance(player.transform.position, animator.transform.position);
        if (distance < attackDistance && distance > scaredDistance)
        {
            spikeLogic.Attack();
        }
        else if (distance > attackDistance)
        {
            animator.SetBool("Attack",false);
        }
        else if (distance < scaredDistance)
        {
            animator.SetBool("PlayerTooClose", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
