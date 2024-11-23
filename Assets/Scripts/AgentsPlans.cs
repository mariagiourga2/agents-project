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

}