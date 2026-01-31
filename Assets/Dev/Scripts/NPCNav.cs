using UnityEngine;
using UnityEngine.AI;


public class NPCNav : MonoBehaviour
{

    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public PlayerMaskState playerMask;

    [Header("Behavior")]
    public float reactRadius = 4f;
    public float fleeDistance = 3f;

    [Header("Flow Settings")]
    public float sideOffsetRange = 0.8f;
    public float decisionInterval = 0.6f;

    float decisionTimer;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.autoBraking = false;

        decisionTimer = Random.Range(0f, decisionInterval);
    }

    void Update()
    {
        if (agent == null || !agent.isOnNavMesh || player == null || playerMask == null)
            return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > reactRadius)
            return;

        decisionTimer -= Time.deltaTime;
        if (decisionTimer > 0f)
            return;

        decisionTimer = decisionInterval;

        if (ShouldApproach())
            ApproachPlayer();
        else
            FleeFromPlayer();
    }

    void ApproachPlayer()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 side = Vector3.Cross(Vector3.up, toPlayer).normalized;

        float sideOffset = Random.Range(-sideOffsetRange, sideOffsetRange);
        Vector3 target = player.position + side * sideOffset;

        agent.SetDestination(target);
    }

    void FleeFromPlayer()
    {
        Vector3 fleeDir = (transform.position - player.position).normalized;

        float angle = Random.Range(-40f, 40f);
        Vector3 angledDir = Quaternion.Euler(0, angle, 0) * fleeDir;

        Vector3 targetPos = transform.position + angledDir * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    bool ShouldApproach()
    {
        
        if (gameObject.name.Contains("Red") && playerMask.currentMask == MaskState.Red)
            return true;

        if (gameObject.name.Contains("Blue") && playerMask.currentMask == MaskState.Blue)
            return true;

        if (gameObject.name.Contains("Purple") && playerMask.currentMask == MaskState.Red)
            return true;

        return false;
    }
}
