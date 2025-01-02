using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class Movement: MonoBehaviour
{
    private NavMeshAgent agent;
    public string homeTag; // Το tag για το "σπίτι" του πράκτορα
    public Transform[] predefinedTargets; // Προκαθορισμένοι στόχοι
    private Queue<Vector3> destinations = new Queue<Vector3>(); // Ουρά προορισμών
    private string plansFilePath;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError($"NavMeshAgent missing on {gameObject.name}. Please attach one.");
            return;
        }

        GameObject home = GameObject.FindWithTag(homeTag);
        if (home != null)
        {
            destinations.Enqueue(home.transform.position); // Αρχικός προορισμός
        }

        plansFilePath = Application.dataPath + "/agents_plans.txt";

        if (File.Exists(plansFilePath))
        {
            LoadDestinationsFromFile(plansFilePath);
        }
        else if (predefinedTargets.Length > 0)
        {
            foreach (Transform target in predefinedTargets)
            {
                destinations.Enqueue(target.position);
            }
        }

        MoveToNextDestination();
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            MoveToNextDestination();
        }
    }

    private void LoadDestinationsFromFile(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (line.StartsWith("("))
                {
                    Vector3 position = ParsePosition(line);
                    if (position != Vector3.zero)
                    {
                        destinations.Enqueue(position);
                    }
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error reading destinations file: {e.Message}");
        }
    }

    private void MoveToNextDestination()
    {
        if (destinations.Count > 0)
        {
            Vector3 nextDestination = destinations.Dequeue();
            agent.SetDestination(nextDestination);
            Debug.Log($"{gameObject.name} moving to {nextDestination}");
        }
        else
        {
            Debug.Log($"{gameObject.name} has no more destinations.");
        }
    }

    private Vector3 ParsePosition(string line)
    {
        try
        {
            string[] parts = line.Trim('(', ')').Split(',');
            float x = float.Parse(parts[0]);
            float y = float.Parse(parts[1]);
            float z = float.Parse(parts[2]);
            return new Vector3(x, y, z);
        }
        catch
        {
            Debug.LogWarning($"Invalid position format: {line}");
            return Vector3.zero;
        }
    }

    public void AssignNewDestination(Vector3 newTarget)
    {
        destinations.Enqueue(newTarget);
        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNextDestination();
        }
    }
}
