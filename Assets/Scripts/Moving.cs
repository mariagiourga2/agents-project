using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public Vector3 homePosition; 
    public Queue<Vector3> plan = new Queue<Vector3>(); 
    public float moveSpeed = 6f; 

    private Vector3 randomDirection;
    private float changeDirectionInterval = 2f;
    private float timeSinceDirectionChange = 0f;

    void Start()
    {
        LoadPlan();
        ChooseRandomDirection(); 
    }

    void Update()
    {
        timeSinceDirectionChange += Time.deltaTime;

        if (timeSinceDirectionChange >= changeDirectionInterval)
        {
            ChooseRandomDirection();
            timeSinceDirectionChange = 0f;
        }

        MoveRandomly();

        CheckPlanTargets();
    }

    private void MoveRandomly()
    {
        transform.position += randomDirection * moveSpeed * Time.deltaTime;
    }

    private void ChooseRandomDirection()
    {
        randomDirection = new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            0,
            UnityEngine.Random.Range(-1f, 1f)
        ).normalized;
    }

    private void LoadPlan()
    {
        string filePath = Path.Combine(Application.dataPath, "agents_plans.txt");
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            bool agentFound = false;

            foreach (string line in lines)
            {
                if (line.StartsWith(name + ":"))
                {
                    agentFound = true;
                    continue;
                }

                if (agentFound)
                {
                    if (string.IsNullOrWhiteSpace(line)) break;

                    string cleanedLine = line.Trim();
                    string[] parts = cleanedLine.Trim(new char[] { '(', ')' }).Split(',');

                    if (parts.Length == 3)
                    {
                        try
                        {
                            float x = float.Parse(parts[0].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                            float y = float.Parse(parts[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                            float z = float.Parse(parts[2].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                            plan.Enqueue(new Vector3(x, y, z));
                            Debug.Log($"Added point ({x}, {y}, {z}) to {name}'s plan");
                        }
                        catch (FormatException ex)
                        {
                            Debug.LogError($"Invalid format in line: {cleanedLine}. Error: {ex.Message}");
                        }
                    }
                }
            }

            if (plan.Count == 0)
            {
                Debug.LogError($"No targets loaded for {name}. Defaulting to random exploration.");
            }
        }
        else
        {
            Debug.LogError("Plan file not found: " + filePath);
        }
    }

    private void CheckPlanTargets()
    {
        if (plan.Count > 0)
        {
            Vector3 target = plan.Peek(); 

            if (Vector3.Distance(transform.position, target) < 1.0f)
            {
                Debug.Log($"{name} reached target {target}");

                plan.Dequeue();
            }
        }
    }
}
