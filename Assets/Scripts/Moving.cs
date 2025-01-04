using System;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using UnityEngine.UIElements;
using System.Collections.Generic;
[RequireComponent(typeof(NavMeshAgent))]

public class Moving : MonoBehaviour
{
    private string filePath;
    public List<AgentPlan> agentsPlans = new List<AgentPlan>(); // Σχέδια για όλους τους πράκτορες
    private List<Transform> agents;    // Πράκτορες στον κόσ

    void Start()
    {
        filePath = Application.dataPath + "/agents_plans.txt";
        ReadPlansFromFile();

        agents = new List<Transform>();

        // Βρες όλους τους NavMeshAgents στη σκηνή
        foreach (var agent in FindObjectsOfType<NavMeshAgent>())
        {
            agents.Add(agent.transform);
        }

        if (agents.Count != agentsPlans.Count)
        {
            Debug.LogError($"Mismatch: {agents.Count} agents in scene, but {agentsPlans.Count} plans loaded from file.");
        }
    }

    void Update()
    {
        /*for (int i = 0; i < agents.Count; i++)
        {
            // Πάρε το σχέδιο του πράκτορα
            AgentPlan plan = agentsPlans[i];
            Transform agentTransform = agents[i];
            NavMeshAgent agent = agentTransform.GetComponent<NavMeshAgent>();

            if (plan.destinations.Count > 0)
            {
                // Πάρε τον τρέχοντα στόχο
                Destination currentTarget = plan.destinations[i];

                if (!currentTarget.visited)
                {
                    // Οδήγησε τον πράκτορα στον προορισμό
                    agent.SetDestination(currentTarget.position);

                    // Έλεγξε αν έφτασε
                    if (Vector3.Distance(agent.transform.position, currentTarget.position) < 1.0f)
                    {
                        currentTarget.visited = true;
                        plan.destinations.RemoveAt(0);
                        Debug.Log($"Agent {plan.name} visited {currentTarget.position}");
                    }
                }
            }
        }*/
        MoveAgent();
    }


    void ReadPlansFromFile()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        AgentPlan currentPlan = null;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.StartsWith("Agent"))
            {
                currentPlan = new AgentPlan { name = line.Trim(':'), destinations = new List<Destination>() };
                agentsPlans.Add(currentPlan);
            }
            else if (currentPlan != null)
            {
                try
                {
                    string cleanLine = line.Trim().Trim('(', ')');
                    string[] parts = cleanLine.Split(',');

                    if (parts.Length == 3)
                    {
                        if (float.TryParse(parts[0].Trim(), out float x) &&
                            float.TryParse(parts[1].Trim(), out float y) &&
                            float.TryParse(parts[2].Trim(), out float z))
                        {
                            currentPlan.destinations.Add(new Destination
                            {
                                position = new Vector3(x, y, z),
                                visited = false
                            });
                            // Debug.Log($"Added destination for {currentPlan.name}: {x}, {y}, {z}");
                        }
                        else
                        {
                            Debug.LogError($"Invalid format in line: {line}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"Unexpected number of components in line: {line}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error parsing line: {line}. Exception: {ex.Message}");
                }
            }
        }
    }
    bool AreAllAgentsDone()
    {
        foreach (var plan in agentsPlans)
        {
            if (plan.destinations.Exists(d => !d.visited))
                return false;
        }
        return true;
    }


    public void MoveAgent()
    {
        while (!AreAllAgentsDone())
        {
            for (int i = 0; i < agentsPlans.Count; i++) // Για κάθε πράκτορα
            {
                AgentPlan plan = agentsPlans[i];
                Transform agent = agents[i];
                NavMeshAgent navMeshAgent = agent.GetComponent<NavMeshAgent>();

                foreach (var destination in plan.destinations)
                {
                    if (!destination.visited)
                    {
                        Vector3 target = destination.position;
                        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                        {
                            target = hit.position;
                            navMeshAgent.SetDestination(target);

                            if (Vector3.Distance(agent.position, target) < 1.0f)
                            {
                                destination.visited = true;
                                Debug.Log($"Agent {i + 1} visited position {target}");
                            }
                        }
                        else
                        {
                            Debug.LogError($"Invalid NavMesh position for agent {i + 1} at {target}");
                        }
                        break; // Ένας προορισμός τη φορά
                    }
                }
            }
        }
        Debug.Log("All agents have completed their plans!");
    }
}
    [System.Serializable]
public class AgentPlan
{
    public string name; // Όνομα του πράκτορα
    public List<Destination> destinations; // Προορισμοί του πράκτορα
}

[System.Serializable]
public class Destination
{
    public Vector3 position; // Συντεταγμένες
    public bool visited;     // Κατάσταση επίσκεψης
}