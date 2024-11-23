using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AgentsPlans : MonoBehaviour
{
    private string filePath1;
    private string filePath2;
    
    void Start()
    {
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
        public string Action {  get; set; }
        public string Description { get; set; }
    }

    private Vector3 ExtractPosition(string line)
    {
        // Παράδειγμα για εξαγωγή θέσης από μια γραμμή
        // "Bakery at position (x: 10.0, y: 0.0, z: 15.0) marked as 'B'"
        string[] parts = line.Split(new[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
        float x = float.Parse(parts[1].Split(':')[1].Trim());
        float y = float.Parse(parts[2].Split(':')[1].Trim());
        float z = float.Parse(parts[3].Split(':')[1].Trim());
        return new Vector3(x, y, z);
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
        // Βρες τον πράκτορα με βάση το όνομά του
        GameObject agent = GameObject.Find($"Agent{agentID}");
        if (agent != null)
        {
            Movement Movement = agent.GetComponent<Movement>();
            if (Movement != null)
            {
                StartCoroutine(ExecuteTargetSequence(Movement, targets));
            }
        }
        else
        {
            Debug.LogWarning($"Agent{agentID} not found in the scene!");
        }
    }

    // Εκτέλεση της ακολουθίας στόχων
    IEnumerator ExecuteTargetSequence(Movement Movement, string[] targets)
    {
        foreach (string target in targets)
        {
            // Εντοπισμός του επόμενου κτιρίου με βάση το συμβολισμό
            GameObject building = FindBuildingBySymbol(target);
            if (building != null)
            {
                Vector3 targetPosition = building.transform.position;
                Movement.SetTarget(targetPosition);

                // Περιμένουμε να φτάσει στον προορισμό
                while (!Movement.HasReachedTarget())
                {
                    yield return null; // Συνεχίζει να περιμένει
                }

                // Μικρή καθυστέρηση (π.χ., παραμονή στο σημείο)
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    // Μέθοδος για αναζήτηση κτιρίων βάσει συμβόλου
    GameObject FindBuildingBySymbol(string symbol)
    {
        // Υποθέτουμε ότι τα κτίρια έχουν συγκεκριμένα tags ή ονόματα
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject building in buildings)
        {
            if (building.name.Contains(symbol)) // Π.χ., "B" για Bakery
            {
                return building;
            }
        }
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