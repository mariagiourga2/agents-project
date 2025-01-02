using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.IO;

public class AgentsIdle : MonoBehaviour
{
    private NavMeshAgent agent;
    public string homeTag;
    private Transform homePosition;
    private Queue<Vector3> destinations = new Queue<Vector3>();
    private Dictionary<string, List<Vector3>> agentsDestinations = new Dictionary<string, List<Vector3>>();

    private string agentName = "DefaultAgent";

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject home = GameObject.FindWithTag(homeTag);
        if (home != null)
        {
            homePosition = home.transform;
            destinations.Enqueue(homePosition.position); // Αρχικός προορισμός
        }

        // Εκκίνηση μετακίνησης
        MoveToNextDestination();
    }

    void Update()
    {
        // Εάν ο πράκτορας έφτασε στον προορισμό
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNextDestination();
        }
    }

    public void LoadDestinationsFromFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            if (line.StartsWith("Agent"))
            {
                agentName = line.Replace(":", "").Trim();
                agentsDestinations[agentName] = new List<Vector3>();
            }
            else if (line.StartsWith("("))
            {
                string[] parts = line.Trim('(', ')').Split(',');
                float x = float.Parse(parts[0]);
                float y = float.Parse(parts[1]);
                float z = float.Parse(parts[2]);
                agentsDestinations[agentName].Add(new Vector3(x, y, z));
            }
        }
    }

    public void AssignNewDestination(Vector3 target)
    {
        destinations.Enqueue(target);
        Debug.Log($"Assigned new target: {target}");
        // Εάν ο πράκτορας δεν είναι ήδη σε κίνηση, ξεκίνα άμεσα
        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNextDestination();
        }
    }

    public void MoveToNextDestination()
    {
        if (destinations.Count > 0)
        {
            Vector3 nextDestination = destinations.Dequeue();
            agent.SetDestination(nextDestination);
            Debug.Log($"Moving {agentName} to {nextDestination}");
        }
        else
        {
            Debug.Log($"{agentName}: No more destinations.");
        }
    }
}
