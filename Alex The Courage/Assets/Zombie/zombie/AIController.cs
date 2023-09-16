
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    
    private PlayerRagdoll playerRagdoll;
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
      
        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        patrolTarget = GetRandomPatrolTarget();
        playerRagdoll = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRagdoll>();
    }

    private void Update()
    {
        
       
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
        transform.LookAt(player);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("Attack");
            agent.SetDestination(transform.position);

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



    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter called. Collided with: " + collision.gameObject.tag);

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Enemy hit player");

            if (playerRagdoll != null)
            {
                playerRagdoll.EnableRagdoll();
            }
           
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





/*void Start()
{
    playerRagdoll = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRagdoll>();
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    m_PlayerPosition = Vector3.zero;
    m_IsPatrol = true;
    m_CaughtPlayer = false;
    m_playerInRange = false;
    m_PlayerNear = false;
    m_WaitTime = startWaitTime;
    m_TimeToRotate = timeToRotate;

    m_CurrentWaypointIndex = 0;
    navMeshAgent = GetComponent<NavMeshAgent>();

    animate = GetComponent<Animator>();

    if (waypoints.Length > 0) // Added a check here to ensure waypoints are set.
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }
}*/




/* public void EndOfAttack()
    {
        Debug.Log("Attack animation finished. Enabling player ragdoll.");
        playerRagdoll.EnableRagdoll();
    }*/

/*private void Chasing()
{
    m_PlayerNear = false;
    m_IsPatrol = false;

    if (playerRagdoll.IsRagdollEnabled)
    {
        // Stop all AI activities if the player is in ragdoll mode
        Stop();
        return;
    }

    // Check if enemy is close enough to the player to start attacking
    if (Vector3.Distance(transform.position, playerTransform.position) < 1.8f) // 3.0f is the attack range
    {
        animate.SetBool("IsAttacking", true);
        animate.SetBool("IsMoving", false);  // Stop the enemy from moving when attacking

        return;  // Return from the function, effectively preventing the code below from running
    }
    else
    {
        animate.SetBool("IsAttacking", false);
    }

    // If the AI hasn't caught the player yet, continue chasing
    if (!m_CaughtPlayer)
    {
        Move(speedRun);
        navMeshAgent.SetDestination(playerTransform.position);
    }

    // If AI has reached the player's last known position and still doesn't see the player, return to patrolling
    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
    {
        if (!m_playerInRange)
        {
            m_IsPatrol = true;
            return;
        }
        HandleStoppingDistance();

    }

}*/


/* private void HandleStoppingDistance()
    {
        if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
        {
            m_IsPatrol = true;
            m_PlayerNear = false;
            Move(speedWalk);
            m_TimeToRotate = timeToRotate;
            m_WaitTime = startWaitTime;
            if (waypoints.Length > 0 && m_CurrentWaypointIndex < waypoints.Length) // Added a check here
            {
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
            {
                Stop();
            }
            m_WaitTime -= Time.deltaTime;
        }
    }*/

/*private void HandlePlayerNear()
   {
       if (m_PlayerNear)
       {
           if (m_TimeToRotate <= 0)
           {
               Move(speedWalk);
               LookingPlayer(playerLastPosition);
           }
           else
           {
               Stop();
               m_TimeToRotate -= Time.deltaTime;
           }
       }
   }*/

/* private void HandlePatrolStoppingDistance()
 {
     if (m_WaitTime <= 0)
     {
         NextPoint();
         Move(speedWalk);
         m_WaitTime = startWaitTime;
     }
     else
     {
         Stop();
         m_WaitTime -= Time.deltaTime;
     }
 }
*/
/* public void NextPoint()
 {
     if (waypoints.Length == 0) return; // Guard against empty array
     m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
     navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
 }

 void Move(float speed)
 {
     navMeshAgent.isStopped = false;
     navMeshAgent.speed = speed;
     animate.SetBool("IsMoving", true);
 }

 void Stop()
 {
     navMeshAgent.isStopped = true;
     navMeshAgent.speed = 0;
     animate.SetBool("IsMoving", false);
     animate.SetBool("IsAttacking", false);
 }



 void CaughtPlayer()
 {
     m_CaughtPlayer = true;
 }

 void LookingPlayer(Vector3 player)
 {
     navMeshAgent.SetDestination(player);
     if (Vector3.Distance(transform.position, player) <= 0.3)
     {
         HandleLookPlayerStoppingDistance();
     }
 }

 private void HandleLookPlayerStoppingDistance()
 {
     if (m_WaitTime <= 0)
     {
         m_PlayerNear = false;
         Move(speedWalk);
         if (waypoints.Length > 0 && m_CurrentWaypointIndex < waypoints.Length) // Added a check here
         {
             navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
         }
         m_WaitTime = startWaitTime;
         m_TimeToRotate = timeToRotate;
     }
     else
     {
         Stop();
         m_WaitTime -= Time.deltaTime;
     }
 }

 void EnviromentView()
 {
     Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius

     for (int i = 0; i < playerInRange.Length; i++)
     {
         Transform player = playerInRange[i].transform;
         Vector3 dirToPlayer = (player.position - transform.position).normalized;
         if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
         {
             float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
             if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
             {
                 m_playerInRange = true;             //  The player has been seeing by the enemy and then the nemy starts to chasing the player
                 m_IsPatrol = false;                 //  Change the state to chasing the player
             }
             else
             {
                 *//*
                  *  If the player is behind a obstacle the player position will not be registered
                  * *//*
                 m_playerInRange = false;
             }
         }
         if (Vector3.Distance(transform.position, player.position) > viewRadius)
         {
             *//*
              *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
              *  Or the enemy is a safe zone, the enemy will no chase
              * *//*
             m_playerInRange = false;                //  Change the sate of chasing
         }
         if (m_playerInRange)
         {
             *//*
              *  If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
              * *//*
             m_PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
         }
     }
 }*/