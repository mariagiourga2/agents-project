using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    private NavMeshAgent agent; // Αναφορά στον NavMeshAgent
    public Transform target;   // Προορισμός του πράκτορα

    void Start()
    {
        // Εξασφαλίζουμε ότι το αντικείμενο έχει NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"NavMeshAgent missing on {gameObject.name}. Please attach one.");
            return;
        }

        Debug.Log($"{gameObject.name} NavMeshAgent initialized.");

        // Αν έχει οριστεί προορισμός, ορίζουμε τη θέση
        if (target != null)
        {
            agent.SetDestination(target.position);
            Debug.Log($"{gameObject.name}: Initial destination set to {target.position}");
        }
        else
        {
            Debug.LogWarning($"Target is not assigned for {gameObject.name}. Set a target to move the agent.");
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        if (agent == null)
        {
            Debug.LogError($"{gameObject.name}: NavMeshAgent not initialized!");
            return;
        }

        agent.SetDestination(targetPosition);
        Debug.Log($"{gameObject.name}: Target set to {targetPosition}");
    }

    public bool HasReachedTarget()
    {
        if (agent == null)
        {
            Debug.LogError($"{gameObject.name}: NavMeshAgent is null!");
            return false;
        }

        // Ελέγχει αν ο πράκτορας δεν έχει εκκρεμή μονοπάτια και έχει φτάσει στη θέση
        bool hasReached = !agent.pathPending &&
                          agent.remainingDistance <= agent.stoppingDistance &&
                          (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);

        Debug.Log($"{gameObject.name}: HasReachedTarget: {hasReached}, RemainingDistance: {agent.remainingDistance}");
        return hasReached;
    }
}