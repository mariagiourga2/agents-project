using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentsIdle : MonoBehaviour
{
    private NavMeshAgent agent;
    public string homeTag; // Το Tag του σπιτιού που ανήκει στον πράκτορα
    private Transform homePosition; // Η θέση του σπιτιού
    private Queue<Vector3> destinations = new Queue<Vector3>(); // Σειρά στόχων
    private bool travelingToTarget = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"NavMeshAgent missing on {gameObject.name}");
            return;
        }

        // Βρίσκουμε το σπίτι του πράκτορα με βάση το Tag
        GameObject home = GameObject.FindWithTag(homeTag);
        if (home == null)
        {
            Debug.LogError($"Home with tag {homeTag} not found for {gameObject.name}");
            return;
        }

        homePosition = home.transform;

        // Ορίζουμε την αρχική θέση αδράνειας στο σπίτι
        agent.SetDestination(homePosition.position);
        Debug.Log($"{gameObject.name} ξεκινά από το σπίτι του: {homePosition.position}");
    }

    void Update()
    {
        // Αν ο πράκτορας φτάσει στον τρέχοντα στόχο, μεταβαίνει στον επόμενο
        if (!travelingToTarget && destinations.Count > 0)
        {
            Vector3 nextDestination = destinations.Dequeue();
            agent.SetDestination(nextDestination);
            travelingToTarget = true;
            Debug.Log($"{gameObject.name} κατευθύνεται στη θέση: {nextDestination}");
        }

        // Ελέγχουμε αν έφτασε στον στόχο
        if (travelingToTarget && HasReachedTarget())
        {
            travelingToTarget = false;
            Debug.Log($"{gameObject.name} έφτασε στον προορισμό!");
        }
    }

    public void AssignNewDestination(Vector3 target)
    {
        destinations.Enqueue(target);
        Debug.Log($"{gameObject.name} έλαβε νέο στόχο: {target}");
    }

    private bool HasReachedTarget()
    {
        return !agent.pathPending &&
               agent.remainingDistance <= agent.stoppingDistance &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
}
