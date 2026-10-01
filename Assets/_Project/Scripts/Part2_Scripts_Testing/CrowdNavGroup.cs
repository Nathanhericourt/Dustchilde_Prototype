using UnityEngine;
using UnityEngine.AI;

public class CrowdNavGroup : MonoBehaviour
{
    [Tooltip("How far apart (in a circle) each person spreads out around the destination point.")]
    [SerializeField] private float spreadRadius = 1.5f;

    private NavMeshAgent[] agents;

    private void Awake()
    {
        agents = GetComponentsInChildren<NavMeshAgent>();
    }

    public void MoveTo(Vector3 destination)
    {
        int count = agents.Length;

        for (int i = 0; i < count; i++)
        {
            if (agents[i] == null) continue;
            
            // Spread each person around the destination in a circle instead of one exact point
            float angle = (360f / count) * i;
            Vector3 offset = Quaternion.Euler(0f, angle, 0f) * (Vector3.forward * spreadRadius);
            Vector3 point = destination + offset;

            // Make sure that spot is actually walkable - snap back to the destination if not
            if (NavMesh.SamplePosition(point, out NavMeshHit hit, spreadRadius + 1f, NavMesh.AllAreas))
            {
                agents[i].SetDestination(hit.position);
            }
            else
            {
                agents[i].SetDestination(destination);
            }
        }
            
    }
}
