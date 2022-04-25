using UnityEngine;
using UnityEngine.AI;

public class AgentController : Photon.MonoBehaviour
{
    public enum AgentState
    {
        Idle = 0,
        Patrolling,
        Chasing
    }

    public AgentState state;
    public Transform[] waypoints;
    private NavMeshAgent navMeshAgent;
    private Animator animController;
    private int speedHashId;
    [SerializeField]
    private int distanceToStartHeadingToNextWaypoint = 2;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private int distanceToStartChasingTarget = 7;
    [SerializeField]
    private float distanceToStartAttackingTarget = 5;
    public float rotationSpeed = 2.0f;
    private Vector3 correctEnemyPos;
    private Quaternion correctEnemyRot;

    void Awake()
    {
        correctEnemyPos = transform.position;
        correctEnemyRot = transform.rotation;

        speedHashId = Animator.StringToHash("walkingSpeed");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animController = GetComponent<Animator>();

        if (waypoints.Length == 0)
        {
            Debug.LogError("Error: list of waypoints is empty.");
        }

        navMeshAgent.SetDestination(waypoints[currentDestination].position);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this enemy: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext((int)state);
        }
        else if (stream.isReading)
        {
            // Network enemy, receive data
            correctEnemyPos = (Vector3)stream.ReceiveNext();
            correctEnemyRot = (Quaternion)stream.ReceiveNext();
            state = (AgentState)stream.ReceiveNext();
        }
    }

    void Update()
    {
        if (PhotonNetwork.inRoom && !photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctEnemyPos, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Slerp(transform.rotation, correctEnemyRot, Time.deltaTime * 10f);
            return;
        }

        // We want to prevent constantly switching targets
        if (target == null)
        {
            // We don't have a target so find a new one
            state = AgentState.Patrolling;
            GameObject found = FindPlayerInRange();
            if (found != null) target = found.transform;
        }
        else if (InRange(gameObject, target.gameObject))
        {
            // The target is still in range so chase them
            state = AgentState.Chasing;
        }
        else
        {
            // If we're no longer in range, find a new target
            target = null;
            state = AgentState.Patrolling;
        }

        if (state == AgentState.Idle)
            Idle();
        else if (state == AgentState.Patrolling)
            Patrol();
        else
            Chase();
    }

    bool InRange(GameObject enemy, GameObject target)
    {
        if (enemy == null || target == null) return false;
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, target.transform.position);
        return distanceToPlayer < distanceToStartChasingTarget;
    }

    GameObject FindPlayerInRange()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (InRange(gameObject, player))
            {
                return player;
            }
        }
        return null;
    }

    void Chase()
    {
        navMeshAgent.SetDestination(target.position);
        navMeshAgent.stoppingDistance = 2;

        if (navMeshAgent.remainingDistance <= 2)
        {
            Idle();
        }
        else if (navMeshAgent.remainingDistance < distanceToStartChasingTarget)
        {
            animController.SetFloat(speedHashId, 1.0f);
            navMeshAgent.isStopped = false;
            RoateTowardsTarget();
            if (navMeshAgent.remainingDistance < distanceToStartAttackingTarget)
            {
                animController.SetTrigger("attack");
            }
        }
    }

    void RoateTowardsTarget()
    {
        Vector3 planarDifference = (target.position - transform.position);
        planarDifference.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(planarDifference.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Idle()
    {
        animController.SetFloat(speedHashId, 0.0f);
        navMeshAgent.isStopped = true;
    }

    private int currentDestination = 0;
    void Patrol()
    {
        animController.SetFloat(speedHashId, 1.0f);
        navMeshAgent.isStopped = false;
        float remainingDistance = navMeshAgent.remainingDistance;
        if (remainingDistance < distanceToStartHeadingToNextWaypoint)
        {
            currentDestination = (currentDestination + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[currentDestination].position);
        }
    }
}