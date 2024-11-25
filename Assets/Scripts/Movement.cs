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
        /*agent = GetComponent<NavMeshAgent>();
        if (target != null)
        {
            agent.SetDestination(target.position);
        }*/
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"NavMeshAgent missing on {gameObject.name}");
        }
        else
        {
            Debug.Log($"{gameObject.name} NavMeshAgent initialized.");
        }
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        if (agent != null)
        {
            Debug.Log($"Setting target for {gameObject.name} to {targetPosition}");
            agent.SetDestination(targetPosition);
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = targetPosition;
            sphere.transform.localScale = Vector3.one * 0.5f;
            Destroy(sphere, 2f); // Remove after 2 seconds
        }
        else
        {
            Debug.LogError("NavMeshAgent is null!");
        }
    }



    public bool HasReachedTarget()
    {
        if (agent == null) return false;

        // Επιστρέφει true αν ο πράκτορας είναι κοντά στον στόχο
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
}
