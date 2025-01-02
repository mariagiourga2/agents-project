using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    private NavMeshAgent agent; // Αναφορά στον NavMeshAgent
    public Transform[] targets;   // Προορισμοί (στόχοι) από τη λίστα
    private int currentTargetIndex = 0; // Δείκτης για τον τρέχοντα στόχο

    void Start()
    {
        // Εξασφαλίζουμε ότι το αντικείμενο έχει NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            //Debug.LogError($"NavMeshAgent missing on {gameObject.name}. Please attach one.");
            return;
        }

        //Debug.Log($"{gameObject.name} NavMeshAgent initialized.");//edv

        // Αν υπάρχουν στόχοι, θέτουμε τον αρχικό προορισμό
        if (targets.Length > 0)
        {
            agent.SetDestination(targets[currentTargetIndex].position);
            Debug.Log($"{gameObject.name}: Initial destination set to {targets[currentTargetIndex].position}");
        }
        else
        {
            Debug.LogWarning($"No targets assigned for {gameObject.name}. Set targets to move the agent.");
        }
    }

    void Update()
    {
        // Αν ο πράκτορας έχει φτάσει στον τρέχοντα στόχο, προχωρά στον επόμενο
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            // Όταν φτάσει στον τρέχοντα στόχο, προχωράμε στον επόμενο
            currentTargetIndex++;
            if (currentTargetIndex < targets.Length)
            {
                agent.SetDestination(targets[currentTargetIndex].position);
                Debug.Log($"{gameObject.name}: Moving to next target: {targets[currentTargetIndex].position}");
            }
            else
            {
               // Debug.Log($"{gameObject.name}: All targets reached.");
            }
        }
    }


    public void SetTarget(Vector3 targetPosition)
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.SetDestination(targetPosition);
            Debug.Log($"{gameObject.name} has new target: {targetPosition}");
        }
        else
        {
            Debug.LogWarning($"Agent {gameObject.name} is not active or does not have NavMeshAgent!");
        }
    }


    public bool HasReachedTarget()
    {
        bool hasReached = !agent.pathPending &&
                          agent.remainingDistance <= agent.stoppingDistance &&
                          (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);

        Debug.Log($"{gameObject.name}: HasReachedTarget: {hasReached}, RemainingDistance: {agent.remainingDistance}");
        return hasReached;
    }

}