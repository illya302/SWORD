using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ThiefIdle : StateMachineBehaviour
{
    private float travelTimeMax;
    private float travelDistance;
    private float lootDistance;
    private float chaseDistance;
    private float travelTime;
    private float defaultSpeed;

    private GameObject thief;
    private ThiefLogic thiefLogic;
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        thief = animator.gameObject;
        player = GameObject.Find("Hero");
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        thiefLogic = animator.GetComponent<ThiefLogic>();

        travelTimeMax = thiefLogic.travelTimeMax;
        travelDistance = thiefLogic.travelDistance;
        chaseDistance = thiefLogic.chaseDistance;
        defaultSpeed = thiefLogic.defaultSpeed;
        lootDistance = thiefLogic.lootingDistance;

        navMeshAgent.speed = defaultSpeed;

        Vector3 destination = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
        navMeshAgent.SetDestination(thief.transform.position + (destination * travelDistance));
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Collider2D[] allLoot = Physics2D.OverlapCircleAll(thief.transform.position, lootDistance);
        List<GameObject> allWantedLoot = new List<GameObject>();
        travelTime -= Time.deltaTime;

        foreach (Collider2D l in allLoot) 
        {
            if (l.TryGetComponent(out HealParticleLogic heal) || l.TryGetComponent(out ExpParticleLogic experience)) 
            {
                allWantedLoot.Add(l.gameObject);
            }
        }

        if (allWantedLoot.Count() != 0) 
        {
            allWantedLoot = allWantedLoot.OrderBy(v => Vector3.Distance(v.transform.position, thief.transform.position)).ToList();
            thiefLogic.lootTarget = allWantedLoot[0];
            navMeshAgent.ResetPath();
            animator.SetBool("Looting", true);
        }
        if (player != null && Vector3.Distance(player.transform.position, thief.transform.position) < chaseDistance)
        {
            navMeshAgent.ResetPath();
            animator.SetBool("Attack", true);
        }
        if (travelTime < 0)
        {
            travelTime = travelTimeMax;
            Vector3 destination = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
            navMeshAgent.SetDestination(thief.transform.position + (destination * travelDistance));
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
