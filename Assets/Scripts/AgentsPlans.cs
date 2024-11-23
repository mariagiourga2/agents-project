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
            //Διαβαζει τον χαρτη
            List<string> cityDescription = new List<string>(File.ReadAllLines(filePath1));

            List<string> importantBuildings = new List<string>();
            foreach (string line in cityDescription)
            {
                if (line.Contains("Bakery")) importantBuildings.Add("B");
                if (line.Contains("Super Market")) importantBuildings.Add("M");
                if (line.Contains("Stadium")) importantBuildings.Add("S");
                if (line.Contains("Drug Store")) importantBuildings.Add("D");
                if (line.Contains("Gas Station")) importantBuildings.Add("G");
                if (line.Contains("Factory")) importantBuildings.Add("F");
            }
            List<string> agentsPlans = new List<string>();
            int agentCount = 5; // Αριθμός πρακτόρων

            for (int i = 1; i <= agentCount; i++)
            {
                agentsPlans.Add($"BEGINPLAN Agent {i}");

                // Δημιουργία τυχαίας σειράς κτιρίων
                List<string> randomPlan = new List<string>(importantBuildings);
                Shuffle(randomPlan);
                agentsPlans.Add(string.Join(" ", randomPlan));

                agentsPlans.Add("ENDPLAN");
            }

            // Εξαγωγή σε αρχείο
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
}