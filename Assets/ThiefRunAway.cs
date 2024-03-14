using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ThiefRunAway : StateMachineBehaviour
{
    private GameObject thief;
    private ThiefLogic thiefLogic;
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    private float chaseDistance;
    private float scaredDistance;
    private float chasingSpeed;
    private float lootingDistance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        thief = animator.gameObject;
        player = GameObject.Find("Hero");
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        thiefLogic = animator.GetComponent<ThiefLogic>();

        chaseDistance = thiefLogic.chaseDistance;
        scaredDistance = thiefLogic.scaredDistandce;
        chasingSpeed = thiefLogic.chasingSpeed;
        lootingDistance = thiefLogic.lootingDistance;

        navMeshAgent.speed = chasingSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(player.transform.position, animator.transform.position);

        Collider2D[] allLoot = Physics2D.OverlapCircleAll(thief.transform.position, lootingDistance);
        List<GameObject> allWantedLoot = new List<GameObject>();
        foreach (Collider2D l in allLoot)
        {
            if (l.TryGetComponent(out HealParticleLogic heal) || l.TryGetComponent(out ExpParticleLogic experience))
            {
                allWantedLoot.Add(l.gameObject);
            }
        }
        if (distance < scaredDistance)
        {
            navMeshAgent.SetDestination(-1 * (player.transform.position - animator.transform.position) * 10);
        }
        else if (distance > chaseDistance && allWantedLoot.Count != 0 && Vector3.Distance(thief.transform.position, thiefLogic.lootTarget.transform.position) < lootingDistance)
        {
            navMeshAgent.ResetPath();
            allWantedLoot = allWantedLoot.OrderBy(l => Vector3.Distance(animator.transform.position, player.transform.position)).ToList();
            thiefLogic.lootTarget = allWantedLoot[0];
            animator.SetBool("Looting", true);
            animator.SetBool("RunAway", false);
        }
        else if (distance > scaredDistance && distance < chaseDistance)
        {
            thiefLogic.lootTarget = null;
            navMeshAgent.ResetPath();
            animator.SetBool("Attack", true);
            animator.SetBool("RunAway", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
