using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    /* private string filePath;
     public List<AgentPlan> agentsPlans = new List<AgentPlan>(); // Σχέδια για όλους τους πράκτορες
     private List<Transform> agents;    // Πράκτορες στον κόσμο

     public GameObject visitedPrefab; // Προαιρετικό prefab για να δείξεις ότι επισκέφθηκε ο προορισμός

     void Start()
     {
         filePath = Application.dataPath + "/agents_plans.txt";
         ReadPlansFromFile();
         InitializeAgents();

         // Εκκίνηση της κίνησης
         StartCoroutine(MoveAgents());
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
                             Debug.Log($"Added destination for {currentPlan.name}: {x}, {y}, {z}");
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

     void InitializeAgents()
     {
         agents = new List<Transform>();
         GameObject[] agentObjects = GameObject.FindGameObjectsWithTag("A");

         foreach (GameObject agentObject in agentObjects)
         {
             string agentName = agentObject.name;
             AgentPlan plan = agentsPlans.Find(p => agentName.Contains(p.name));

             if (plan != null)
             {
                 agents.Add(agentObject.transform);
                 Debug.Log($"Assigned plan to {agentName}");
             }
             else
             {
                 Debug.LogError($"Agent {agentName} does not have a corresponding plan!");
             }
         }

         if (agents.Count != agentsPlans.Count)
         {
             Debug.LogError($"Mismatch between agents in the scene and plans in the file! Scene agents: {agents.Count}, File plans: {agentsPlans.Count}");
         }
     }

     IEnumerator MoveAgents()
     {
         bool allVisited = false;

         while (!allVisited)
         {
             allVisited = true;

             for (int j = 0; j < agents.Count; j++) // Για κάθε πράκτορα
             {
                 AgentPlan plan = agentsPlans[j];
                 Transform agent = agents[j];
                 NavMeshAgent navMeshAgent = agent.GetComponent<NavMeshAgent>();

                 if (navMeshAgent == null)
                 {
                     Debug.LogError($"Agent {agent.name} does not have a NavMeshAgent component!");
                     continue;
                 }

                 for (int i = 0; i < plan.destinations.Count; i++) // Για κάθε προορισμό
                 {
                     if (!plan.destinations[i].visited)
                     {
                         Vector3 target = plan.destinations[i].position;

                         // Ελέγξτε αν ο προορισμός βρίσκεται στο NavMesh
                         if (NavMesh.SamplePosition(target, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                         {
                             target = hit.position; // Ενημερώστε τον προορισμό με την έγκυρη θέση
                         }
                         else
                         {
                             Debug.LogError($"Destination {target} is invalid for agent {j + 1}");
                             continue; // Παραλείψτε τον συγκεκριμένο προορισμό
                         }

                         // Ενημέρωση του NavMeshAgent με τη νέα θέση
                         navMeshAgent.SetDestination(target);

                         // Αν έφτασε στον προορισμό
                         if (Vector3.Distance(agent.position, target) < 1.0f)
                         {
                             plan.destinations[i].visited = true;
                             Debug.Log($"Agent {j + 1} visited position {target}");

                             if (visitedPrefab != null)
                             {
                                 Instantiate(visitedPrefab, target, Quaternion.identity);
                             }
                         }

                         allVisited = false; // Υπάρχουν ακόμα μη επισκεπτόμενοι προορισμοί
                         break; // Προχωράμε μόνο έναν προορισμό τη φορά
                     }
                 }
             }

                 yield return null; // Περιμένουμε το επόμενο frame
         }

         Debug.Log("All destinations visited!");
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
 */
}
