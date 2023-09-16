
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerRagdoll playerRagdoll;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    private Animator animator;
    private Rigidbody rb;

    // Patrolling
    public Transform patrolCenterPoint;
    public float patrolRange;
    private Vector3 patrolTarget;
    private bool isPatrolling = true;

    // Attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    void Start()
    {
        playerRagdoll = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRagdoll>();

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        patrolTarget = GetRandomPatrolTarget();
       
    }

    private void Update()
    {

        if (playerRagdoll == null)
        {
            playerRagdoll = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRagdoll>();
        }
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
   
    private void Chasing()
    {
       
        isPatrolling = false;
        agent.SetDestination(player.position);
        {
            agent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, patrolCenterPoint.position) > patrolRange)
            {
                isPatrolling = true;
                agent.SetDestination(patrolCenterPoint.position);
            }
        }
    }
    private void Attacking()
    {
       

        Quaternion rotationToLookAt = Quaternion.LookRotation(player.position - transform.position);
        float rotationSpeed = 10f;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToLookAt, Time.deltaTime * rotationSpeed);


        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("Attack");
            agent.SetDestination(transform.position);
            FindObjectOfType<AudioManager>().Play("Zombie Attack");
        }
    }

    private void ResetAttack()
    {
        
        Debug.Log("ATTACK RESET");
       
        alreadyAttacked = false;
       
    }


    private void Patrolling()
    {
        
        if (agent.remainingDistance < 1.0f)
        {
            patrolTarget = GetRandomPatrolTarget();
            agent.SetDestination(patrolTarget);
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

    void OnTriggerEnter(Collider collider)
    {
        GameObject hit = collider.gameObject;
        if (hit.CompareTag("Player"))
        {
            PlayerRagdoll playerRagdoll = hit.GetComponent<PlayerRagdoll>();
            if (playerRagdoll != null && !playerRagdoll.IsRagdollEnabled)
            {
                Debug.Log("Enabling Ragdoll");
                playerRagdoll.EnableRagdoll();
                FindObjectOfType<AudioManager>().Play("Hit");
                TriggerRespawn();
            }
        }
    }

    void TriggerRespawn()
    {
        Debug.Log("Triggering Respawn");
        if (gameManager != null)
        {
            gameManager.EndGame();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

