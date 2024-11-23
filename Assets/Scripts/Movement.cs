using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        if (agent != null)
        {
            agent.SetDestination(targetPosition);
        }
    }

    public bool HasReachedTarget()
    {
        if (agent == null) return false;

        // Επιστρέφει true αν ο πράκτορας είναι κοντά στον στόχο
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
}
