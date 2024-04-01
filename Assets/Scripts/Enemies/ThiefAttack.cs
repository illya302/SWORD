using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThiefAttack : StateMachineBehaviour
{
    private GameObject thief;
    private ThiefLogic thiefLogic;
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    private float chaseDistance;
    private float attackDistance;
    private float chasingSpeed;
    private float scaredDistance;
    private float lootDistance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        thief = animator.gameObject;
        player = GameObject.Find("Hero");
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        thiefLogic = animator.GetComponent<ThiefLogic>();

        chaseDistance = thiefLogic.chaseDistance;
        attackDistance = thiefLogic.attackDistance;
        chasingSpeed = thiefLogic.chasingSpeed;
        scaredDistance = thiefLogic.scaredDistandce;
        lootDistance = thiefLogic.lootingDistance;

        navMeshAgent.speed = chasingSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Collider2D[] allLoot = Physics2D.OverlapCircleAll(thief.transform.position, lootDistance);
        float distance = Vector3.Distance(player.transform.position, animator.transform.position);

        foreach (Collider2D l in allLoot)
        {
            if (l.TryGetComponent(out HealParticleLogic heal) || l.TryGetComponent(out ExpParticleLogic experience))
            {
                animator.SetBool("Attack", false);
                break;
            }
        }

        if (distance < attackDistance && distance > scaredDistance)
        {
            thiefLogic.Attack();
            navMeshAgent.ResetPath();
        }
        else if (distance > attackDistance && distance < chaseDistance)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        else if (distance < scaredDistance) 
        {
            animator.SetBool("RunAway", true);
        }
        else if (distance > chaseDistance)
        {
            navMeshAgent.ResetPath();
            animator.SetBool("Attack", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
