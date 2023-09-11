//using System;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class CollisionEnemyAI : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    private Animator animator;



    //Patrolling 
    int wayPointIndex;
    private bool isPatrolling = true;
    public float patrolRange = 100.0f;
    public Transform patrolCenterPoint;
    private Vector3 patrolTarget;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    //public float timer = 3.0f;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        patrolTarget = GetRandomPatrolTarget();

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            isPatrolling = true;
            Patrolling();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            isPatrolling = false; // Enemy is no longer patrolling
            Chasing();
        }

        if (playerInSightRange && playerInAttackRange)
        {
            isPatrolling = false; // Enemy is no longer patrolling
            Attacking();
        }
    }

    private void Patrolling()
    {

        if (isPatrolling)
        {
            if (agent.remainingDistance < 1.0f) // Check if near the random point
            {
                agent.SetDestination(patrolTarget);
                if (Vector3.Distance(transform.position, patrolTarget) < 50.0f)
                {
                    patrolTarget = GetRandomPatrolTarget();
                }
            }
        }
    }

    private Vector3 GetRandomPatrolTarget()
    {
        Vector3 randomPoint = patrolCenterPoint.position + Random.insideUnitSphere * patrolRange;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 50.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return patrolCenterPoint.position;
    }

    private void Chasing()
    {
        isPatrolling = false;
        agent.SetDestination(player.position);

        // If enemy goes out of patrol range while chasing, stop chasing and return to patrol area
        if (Vector3.Distance(transform.position, patrolCenterPoint.position) > patrolRange)
        {
            isPatrolling = true;
            agent.SetDestination(patrolCenterPoint.position);
        }
    }

    private void Attacking()
    {
        transform.LookAt(player);


        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("Attack");
            agent.SetDestination(transform.position);

        }
        //Make sure enemy doesn't move

    }

    private void ResetAttack()
    {
        Debug.Log("NO ATTACK");

        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
