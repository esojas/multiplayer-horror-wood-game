using UnityEngine;
using UnityEngine.AI;

public class MonsterAIScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    bool alreadyAttack;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public static bool IsChasing { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() => IsChasing = false; // Resets when press play

    private void Awake()
    {
        player = GameObject.Find("PlayerSurvivor_capsule").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnDisable()
    {
        IsChasing = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer) && !PlayerCrouch.IsHidden;
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer) && !PlayerCrouch.IsHidden;
        IsChasing = playerInSightRange;
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange &&  !playerInAttackRange) Chasing();
        if (playerInSightRange && playerInAttackRange) Attacking();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void Attacking()
    {
        // Jumpscare logic here later

        agent.SetDestination(transform.position);

        transform.LookAt(player);
    }

    private void Chasing()
    {
        agent.SetDestination(player.position);
    }
}
