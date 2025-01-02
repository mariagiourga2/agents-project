using System;
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
        if (!File.Exists(filePath1))
        {
            Debug.LogError("city_description.txt not found!");
            return;
        }

        List<string> cityDescription = new List<string>(File.ReadAllLines(filePath1));
        List<AgentGoal> importantGoals = ExtractImportantGoals(cityDescription);

        if (importantGoals.Count == 0)
        {
            Debug.LogWarning("No important goals found in city_description.txt!");
            return;
        }

        int agentCount = 5;
        List<string> agentsPlans = new List<string>();

        for (int i = 1; i <= agentCount; i++)
        {
            agentsPlans.Add($"{GetAgentName(i)}:");
            Shuffle(importantGoals);
            foreach (var goal in importantGoals)
            {
                agentsPlans.Add($"  {goal.TargetPosition}");
            }
        }

        File.WriteAllLines(filePath2, agentsPlans);
        Debug.Log("Agent plans created at " + filePath2);
    }

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

    private string GetAgentName(int index) => "Agent" + index;

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
