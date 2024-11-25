using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class AgentsPlans : MonoBehaviour
{
    private string filePath1;
    private string filePath2;
    // private Dictionary<int, GameObject> agents = new Dictionary<int, GameObject>();
    void Start()
    {
        /* GameObject[] allAgents = GameObject.FindGameObjectsWithTag("A");
         for (int i = 0; i < allAgents.Length; i++)
         {
             agents[i + 1] = allAgents[i]; // Assuming Agent IDs start from 1
         }*/

        filePath1 = Application.dataPath + "/city_description.txt";
        filePath2 = Application.dataPath + "/agents_plans.txt";
        CreatePlans();
        ExecutePlans();
    }

    void CreatePlans()
    {
        try
        {
            if (!File.Exists(filePath1))
            {
                Debug.LogError("city_description.txt not found!");
                return;
            }

            List<string> cityDescription = new List<string>(File.ReadAllLines(filePath1));
            List<AgentGoal> importantGoals = new List<AgentGoal>();

            foreach (string line in cityDescription)
            {
                if (line.Contains("Bakery"))
                    importantGoals.Add(new AgentGoal { TargetPosition = ExtractPosition(line), Action = "Visit", Description = "Visit Bakery" });
                if (line.Contains("Super Market"))
                    importantGoals.Add(new AgentGoal { TargetPosition = ExtractPosition(line), Action = "Visit", Description = "Visit Super Market" });
                if (line.Contains("Stadium"))
                    importantGoals.Add(new AgentGoal { TargetPosition = ExtractPosition(line), Action = "Visit", Description = "Visit Stadium" });
                if (line.Contains("Drug Store"))
                    importantGoals.Add(new AgentGoal { TargetPosition = ExtractPosition(line), Action = "Visit", Description = "Visit Drug Store" });
                if (line.Contains("Gas Station"))
                    importantGoals.Add(new AgentGoal { TargetPosition = ExtractPosition(line), Action = "Visit", Description = "Visit Gas Station" });
                if (line.Contains("Factory"))
                    importantGoals.Add(new AgentGoal { TargetPosition = ExtractPosition(line), Action = "Visit", Description = "Visit Factory" });
            }

            if (importantGoals.Count == 0)
            {
                Debug.LogWarning("No goals found in city_description.txt!");
                return;
            }

            List<string> agentsPlans = new List<string>();
            int agentCount = 5;

            for (int i = 1; i <= agentCount; i++)
            {
                agentsPlans.Add($"BEGINPLAN Agent {i}");
                Shuffle(importantGoals);

                foreach (var goal in importantGoals)
                {
                    agentsPlans.Add($"Action: {goal.Action}, Target: {goal.TargetPosition}, Description: {goal.Description}");
                }

                agentsPlans.Add("ENDPLAN");
            }

            File.WriteAllLines(filePath2, agentsPlans);
            Debug.Log("Agent plans successfully created at " + filePath2);
        }
        catch (Exception e)
        {
            Debug.LogError("Error generating agent plans: " + e.Message);
        }
    }


    // Μέθοδος για τυχαία αναδιάταξη μιας λίστας
    private void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }
    }

    public class AgentGoal
    {
        public Vector3 TargetPosition { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
    }

    private Vector3 ExtractPosition(string line)
    {
        try
        {
            string[] parts = line.Split(new[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
            float x = float.Parse(parts[1].Split(':')[1].Trim());
            float y = float.Parse(parts[2].Split(':')[1].Trim());
            float z = float.Parse(parts[3].Split(':')[1].Trim());
            //return new Vector3(x, y, z);
            Vector3 position = new Vector3(x, y, z);
            Debug.Log($"Extracted position: {position}");
            return position;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error extracting position from line '{line}': {e.Message}");
            return Vector3.zero;
        }
    }
    void ExecutePlans()
    {
        try
        {
            // Διαβάζουμε τα πλάνα
            List<string> agentsPlans = new List<string>(File.ReadAllLines(filePath2));

            int currentAgent = -1; // Ορίζει ποιος πράκτορας εκτελείται
            foreach (string line in agentsPlans)
            {
                if (line.StartsWith("BEGINPLAN"))
                {
                    currentAgent = ExtractAgentID(line); // Βρες το ID του Agent
                }
                else if (line.StartsWith("ENDPLAN"))
                {
                    currentAgent = -1; // Τερματισμός εκτέλεσης πλάνου
                }
                else if (currentAgent != -1)
                {
                    // Ανάλυση της γραμμής του πλάνου (λίστα στόχων)
                    string[] targets = line.Split(' '); // Διαχωρισμός στόχων
                    MoveAgent(currentAgent, targets);  // Μετακίνηση πράκτορα
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error executing agent plans: " + e.Message);
        }
    }
    void MoveAgent(int agentID, string[] targets)
    {
        Debug.Log($"Attempting to move Agent {agentID} with targets: {string.Join(", ", targets)}");
        GameObject agent = GameObject.Find($"Agent{agentID}");
        if (agent != null)
        {
            Movement movement = agent.GetComponent<Movement>();
            if (movement != null)
            {
                Debug.Log($"Starting movement sequence for {agent.name}");
                StartCoroutine(ExecuteTargetSequence(movement, targets));
            }
            else
            {
                Debug.LogError($"Movement script missing on {agent.name}");
            }
        }
        else
        {
            Debug.LogWarning($"Agent{agentID} not found in the scene!");
        }
    }

    /* void MoveAgent(int agentID, string[] targets)
     {
         if (agents.TryGetValue(agentID, out GameObject agent))
         {
             Movement movement = agent.GetComponent<Movement>();
             if (movement != null)
             {
                 Debug.Log($"Starting movement for {agent.name}");
                 StartCoroutine(ExecuteTargetSequence(movement, targets));
             }
             else
             {
                 Debug.LogError($"Movement script missing on {agent.name}");
             }
         }
         else
         {
             Debug.LogWarning($"Agent {agentID} not found!");
         } 
     }*/


    // Εκτέλεση της ακολουθίας στόχων
    IEnumerator ExecuteTargetSequence(Movement movement, string[] targets)
    {
        Debug.Log($"Executing target sequence for {movement.gameObject.name} with {targets.Length} targets.");
        foreach (string target in targets)
        {
            Debug.Log($"Looking for building with symbol: {target}");
            GameObject building = FindBuildingBySymbol(target);
            if (building != null)
            {
                Vector3 targetPosition = building.transform.position;
                Debug.Log($"{movement.gameObject.name} moving to {targetPosition} (Target: {target})");
                movement.SetTarget(targetPosition);

                while (!movement.HasReachedTarget())
                {
                    yield return null; // Wait until the agent reaches the target
                }

                Debug.Log($"{movement.gameObject.name} reached {targetPosition}");
                yield return new WaitForSeconds(1.0f); // Simulate staying at the target
            }
            else
            {
                Debug.LogWarning($"Target {target} not found for {movement.gameObject.name}");
            }
        }
    }



    GameObject FindBuildingBySymbol(string symbol)
        {
            GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
            foreach (GameObject building in buildings)
            {
                if (building.name.Contains(symbol)) // Ensure building name includes the symbol
                {
                    Debug.Log($"Found building {building.name} for symbol {symbol}");
                    return building;
                }
            }
            Debug.LogWarning($"Building with symbol {symbol} not found!");
            return null;
        }


        // Εξαγωγή του ID από το όνομα του πράκτορα
        int ExtractAgentID(string line)
        {
            string[] parts = line.Split(' ');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int id))
            {
                return id;
            }
            return -1; // Αν αποτύχει
        }
    
}