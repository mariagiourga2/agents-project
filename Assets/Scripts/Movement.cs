using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Movement : MonoBehaviour
{
    // Λίστες για τις συντεταγμένες των στόχων για κάθε πράκτορα
    public List<Vector3> Agent1Destinations = new List<Vector3>();
    public List<Vector3> Agent2Destinations = new List<Vector3>();
    public List<Vector3> Agent3Destinations = new List<Vector3>();
    public List<Vector3> Agent4Destinations = new List<Vector3>();
    public List<Vector3> Agent5Destinations = new List<Vector3>();

    // Λεξικό για τις συντεταγμένες των κτιρίων
    private HashSet<Vector3> discoveredBuildings = new HashSet<Vector3>();

    // Προσωρινή κλάση για το στόχο
    private class Destination
    {
        public Vector3 Position;
        public bool IsVisited;

        public Destination(Vector3 position)
        {
            Position = position;
            IsVisited = false;
        }
    }

    // Λεξικό που κρατά τις λίστες των στόχων για κάθε πράκτορα
    private Dictionary<string, List<Destination>> agentDestinations = new Dictionary<string, List<Destination>>();

    // Ταχύτητα κίνησης του πράκτορα
    public float agentSpeed = 3.0f;

    void Start()
    {
        // Φόρτωμα των στόχων από το αρχείο
        LoadDestinationsFromFile("C:/Users/user/source/Unity Projects/agents-project/Assets/agents_plans.txt");
    }

    // Διαβάζει το αρχείο και γεμίζει τα δεδομένα
    private void LoadDestinationsFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        string currentAgent = "";
        List<Destination> destinations = null;

        foreach (var line in lines)
        {
            // Ελέγχουμε αν είναι το όνομα του πράκτορα
            if (line.StartsWith("Agent"))
            {
                currentAgent = line.Trim(':');
                destinations = new List<Destination>();
                agentDestinations[currentAgent] = destinations;
            }
            else
            {
                // Ελέγχουμε για συντεταγμένες και τις προσθέτουμε στη λίστα του πράκτορα
                Vector3 position = ParsePosition(line);
                if (position != Vector3.zero)
                {
                    destinations.Add(new Destination(position));
                }
            }
        }
    }

    // Παράδειγμα συνάρτησης για να διαβάσουμε τις συντεταγμένες από το αρχείο
    private Vector3 ParsePosition(string line)
    {
        try
        {
            line = line.Trim();
            if (line.StartsWith("(") && line.EndsWith(")"))
            {
                string[] parts = line.Substring(1, line.Length - 2).Split(',');

                if (parts.Length != 3)
                {
                    Debug.LogWarning($"Invalid position format: {line}");
                    return Vector3.zero;
                }

                float x = float.Parse(parts[0].Trim());
                float y = float.Parse(parts[1].Trim());
                float z = float.Parse(parts[2].Trim());
                return new Vector3(x, y, z);
            }
            else
            {
                Debug.LogWarning($"Invalid position format (missing parentheses): {line}");
                return Vector3.zero;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Error parsing position: {line}. Exception: {e.Message}");
            return Vector3.zero;
        }
    }

    // Συνάρτηση κίνησης του πράκτορα
    // Συνάρτηση κίνησης του πράκτορα
    private void MoveAgent(string agentName)
    {
        if (!agentDestinations.ContainsKey(agentName))
        {
            Debug.LogError("Agent not found: " + agentName);
            return;
        }

        List<Destination> destinations = agentDestinations[agentName];

        // Κίνηση προς τον επόμενο διαθέσιμο στόχο
        foreach (var destination in destinations)
        {
            if (!destination.IsVisited)
            {
                MoveToDestination(destination, agentName);
                // Έλεγχος αν οι συντεταγμένες του πράκτορα είναι ίδιες με αυτές του προορισμού
                if (transform.position == destination.Position)
                {
                    destination.IsVisited = true;  // Σημειώνουμε ότι ο στόχος επισκεφτεί
                }
                break;  // Σταματάμε τη κίνηση μετά τον πρώτο επισκέψιμο στόχο
            }
        }
    }


    // Κίνηση του πράκτορα προς τον στόχο
    private void MoveToDestination(Destination destination, string agentName)
    {
        Debug.Log($"Agent {agentName} moving to: {destination.Position}");

        Vector3 agentPosition = transform.position;
        Vector3 direction = (destination.Position - agentPosition).normalized;

        float step = agentSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(agentPosition, destination.Position, step);

        // Έλεγχος αν ο πράκτορας έχει φτάσει στον προορισμό
        if (Vector3.Distance(agentPosition, destination.Position) < 0.1f)
        {
            Debug.Log($"Agent {agentName} reached destination: {destination.Position}");
            destination.IsVisited = true;  // Σημειώνουμε ότι έχει φτάσει

            // Αν το σημείο είναι κτίριο, το αποθηκεύουμε και ενημερώνουμε τους άλλους πράκτορες
            if (IsBuilding(destination.Position))
            {
                discoveredBuildings.Add(destination.Position);
                //ShareKnowledge(agentName);
            }
        }
    }

    // Συνάρτηση για να ελέγξουμε αν το σημείο είναι κτίριο
    private bool IsBuilding(Vector3 position)
    {
        // Αντικαταστήστε αυτή τη συνθήκη με την πραγματική λογική για να ανιχνεύσετε κτίρια
        return position.y > 0;  // Παράδειγμα: Εάν το y είναι μεγαλύτερο από 0, θεωρούμε ότι είναι κτίριο
    }

    // Μοιράζεται τις γνώσεις με άλλους πράκτορες
    private void ShareKnowledge(string agentName)
    {
        foreach (var agent in agentDestinations.Keys)
        {
            if (agent != agentName)
            {
                foreach (var destination in agentDestinations[agent])
                {
                    if (discoveredBuildings.Contains(destination.Position) && !destination.IsVisited)
                    {
                        destination.IsVisited = true; // Ενημερώνουμε ότι έχει βρει και αυτό το κτίριο
                    }
                }
            }
        }
    }

    // Κάθε φορά που καλείται το `Update`, οι πράκτορες θα κινούνται
    void Update()
    {
        MoveAgent("Agent1");
        MoveAgent("Agent2");
        MoveAgent("Agent3");
        MoveAgent("Agent4");
        MoveAgent("Agent5");
    }
}
