using UnityEngine;
using UnityEngine.AI;

public class TurtleAIScript : MonoBehaviour
{
    public float roamRadius = 10f;
    public float idleTime = 5f; // Time in seconds between movements

    private NavMeshAgent agent;
    private Animator animator;
    private float timer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = idleTime; // Initialize timer to force immediate movement decision
    }

    private void Update()
    {
        // Count up the timer
        timer += Time.deltaTime;

        // If the turtle has reached its destination or the timer has exceeded the idleTime, find a new destination
        if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance || timer >= idleTime))
        {
            // Reset timer
            timer = 0f;

            // Find a new random destination
            Vector3 newDestination = RandomNavSphere(transform.position, roamRadius);
            agent.SetDestination(newDestination);

            // Activate walk animation
            if (animator != null) animator.SetBool("IsWalking", true);
        }
        else if(agent.remainingDistance > agent.stoppingDistance)
        {
            // Ensure the walk animation is playing while moving
            if (animator != null) animator.SetBool("IsWalking", true);
        }
        else
        {
            // If not moving, switch to idle animation
            if (animator != null) animator.SetBool("IsWalking", false);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);

        return navHit.position;
    }
}
