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
        // Ορισμός διαδρομών αρχείων
        filePath1 = Application.dataPath + "/city_description.txt";
        filePath2 = Application.dataPath + "/agents_plans.txt";

        // Δημιουργία νέων σχεδίων για τους πράκτορες και εκτέλεση αυτών
        CreatePlans();
        ExecutePlans(filePath2);
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

            // Ανάγνωση της περιγραφής της πόλης
            List<string> cityDescription = new List<string>(File.ReadAllLines(filePath1));
            List<AgentGoal> importantGoals = ExtractImportantGoals(cityDescription);

            if (importantGoals.Count == 0)
            {
                Debug.LogWarning("No important goals found in city_description.txt!");
                return;
            }

            // Αριθμός πρακτόρων
            int agentCount = 5;
            List<string> agentsPlans = new List<string>();

            // Δημιουργία σχεδίων για κάθε πράκτορα
            for (int i = 1; i <= agentCount; i++)
            {
                agentsPlans.Add($"{GetAgentName(i)}:");

                Shuffle(importantGoals);

                // Καταγραφή στόχων για κάθε πράκτορα
                foreach (var goal in importantGoals)
                {
                    agentsPlans.Add($"  {goal.TargetPosition.ToString()}");
                }
            }

            // Αποθήκευση των σχεδίων στο αρχείο
            File.WriteAllLines(filePath2, agentsPlans);
            Debug.Log("Agent plans successfully created at " + filePath2);
        }
        catch (Exception e)
        {
            Debug.LogError("Error generating agent plans: " + e.Message);
        }
    }

    // Αποδοχή στόχων από την περιγραφή της πόλης
    List<AgentGoal> ExtractImportantGoals(List<string> cityDescription)
    {
        List<AgentGoal> importantGoals = new List<AgentGoal>();

        foreach (string line in cityDescription)
        {
            if (line.Contains("Bakery"))
                importantGoals.Add(new AgentGoal("Visit", ExtractPosition(line), "Visit Bakery"));
            if (line.Contains("Super Market"))
                importantGoals.Add(new AgentGoal("Visit", ExtractPosition(line), "Visit Super Market"));
            if (line.Contains("Stadium"))
                importantGoals.Add(new AgentGoal("Visit", ExtractPosition(line), "Visit Stadium"));
            if (line.Contains("Drug Store"))
                importantGoals.Add(new AgentGoal("Visit", ExtractPosition(line), "Visit Drug Store"));
            if (line.Contains("Gas Station"))
                importantGoals.Add(new AgentGoal("Visit", ExtractPosition(line), "Visit Gas Station"));
            if (line.Contains("Factory"))
                importantGoals.Add(new AgentGoal("Visit", ExtractPosition(line), "Visit Factory"));
        }

        return importantGoals;
    }

    // Εκτέλεση των στόχων από το αρχείο
    void ExecutePlans(string filePath)
    {
        try
        {
            //Debug.Log($"Executing plans from file: {filePath}");

            if (!File.Exists(filePath))
            {
                Debug.LogError($"File not found: {filePath}");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            AgentsIdle currentAgent = null;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line.Contains(":"))
                {
                    string agentName = line.Split(':')[0].Trim();
                    GameObject agentObject = GameObject.Find(agentName);

                    if (agentObject == null)
                    {
                        Debug.LogWarning($"Agent not found: {agentName}");
                        continue;
                    }

                    currentAgent = agentObject.GetComponent<AgentsIdle>();
                    if (currentAgent == null)
                    {
                        Debug.LogWarning($"AgentsIdle script not found on {agentName}");
                    }
                    continue;
                }

                if (line.Contains("(") && currentAgent != null)
                {
                    Vector3 targetPosition = ParseTarget(line);
                    if (targetPosition != Vector3.zero)
                    {
                        currentAgent.AssignNewDestination(targetPosition);
                        //Debug.Log($"Assigned target {targetPosition} to {currentAgent.gameObject.name}");
                    }
                    else
                    {
                        //Debug.LogWarning($"Invalid target position in line: {line}");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error executing plans: {e.Message}");
        }
    }

    // Ανάλυση στόχου από τη γραμμή κειμένου
    private Vector3 ParseTarget(string line)
    {
        try
        {
            // Αφαίρεση κενών και παρενθέσεων
            string cleanLine = line.Trim().Trim('(', ')');
            string[] parts = cleanLine.Split(',');

            if (parts.Length != 3)
            {
                //Debug.LogWarning($"Invalid target format: {line}");
                return Vector3.zero;
            }

            // Μετατροπή σε float χωρίς στρογγυλοποίηση
            float x = float.Parse(parts[0].Trim(), System.Globalization.CultureInfo.InvariantCulture);
            float y = float.Parse(parts[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);
            float z = float.Parse(parts[2].Trim(), System.Globalization.CultureInfo.InvariantCulture);

            return new Vector3(x, y, z);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to parse target position: {line}. Error: {e.Message}");
            return Vector3.zero;
        }
    }


    // Εύρεση πράκτορα με βάση το όνομα
    Movement FindAgentByName(string name)
    {
        GameObject agentObject = GameObject.Find(name);
        if (agentObject == null)
        {
            //Debug.LogWarning($"No GameObject found with name: {name}");
            return null;
        }

        Movement movement = agentObject.GetComponent<Movement>();
        if (movement == null)
        {
            //Debug.LogWarning($"Movement script not found on {name}");
            return null;
        }

        return movement;
    }

    // Εξαγωγή θέσης από την περιγραφή
    private Vector3 ExtractPosition(string line)
    {
        try
        {
            string[] parts = line.Split(new[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
            float x = float.Parse(parts[1].Split(':')[1].Trim());
            float y = float.Parse(parts[2].Split(':')[1].Trim());
            float z = float.Parse(parts[3].Split(':')[1].Trim());
            return new Vector3(x, y, z);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error extracting position from line '{line}': {e.Message}");
            return Vector3.zero;
        }
    }

    // Ανακατεύουμε τη λίστα
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

    // Σύνθεση ονόματος πράκτορα
    private string GetAgentName(int index) => "Agent" + index;

    // Κλάση στόχου πράκτορα
    public class AgentGoal
    {
        public string Action { get; }
        public Vector3 TargetPosition { get; }
        public string Description { get; }

        public AgentGoal(string action, Vector3 targetPosition, string description)
        {
            Action = action;
            TargetPosition = targetPosition;
            Description = description;
        }
    }

}
