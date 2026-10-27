using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CrowdMemberAnimator : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // Finds the Animator on the child Mourner model automatically
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (agent == null || animator == null) return;

        // Check if the NavMeshAgent is currently moving and has a path
        bool isMoving = agent.hasPath && agent.velocity.magnitude > 0.1f;

        // Set your Animator parameter
        animator.SetBool("IsWalking", isMoving);
    }
}