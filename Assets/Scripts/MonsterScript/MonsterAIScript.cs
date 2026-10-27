using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAIScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    [SerializeField] private int walkingSpeed = 5;
    [SerializeField] private float walkPointTimeout = 5f;
    private float walkPointTimer;
    [SerializeField] private int runningSpeed = 10;

    public LayerMask whatIsGround, whatIsPlayer;

    private Transform noisePos; // Position where player made noise

    //Patrolling
    private Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    bool alreadyAttack;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public static bool hearNoise { get;  set; }
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
        
        agent.speed = playerInSightRange ?  runningSpeed: walkingSpeed;
        IsChasing = playerInSightRange;
        
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange &&  !playerInAttackRange) Chasing();
        if (playerInSightRange && playerInAttackRange) Attacking();
    }

    private void Patroling()
    {
        if (hearNoise) WalkToNoise();
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            walkPointTimer += Time.deltaTime;
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        bool timedOut = walkPointTimer >= walkPointTimeout;

        if (distanceToWalkPoint.magnitude < 1f || timedOut) // Reach Destination
        {
            walkPointSet = false; // Find a new place to walk
        }
    }

    private void WalkToNoise()
    {

        noisePos = player.transform;
        walkPoint = noisePos.position;
        walkPointTimer = 0f;
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPointTimer = 0f;
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
