using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CityMap : MonoBehaviour
{
    // Ρυθμίσεις για τον χάρτη
    public int N = 300;  // Ύψος του χάρτη
    public int M = 260;  // Πλάτος του χάρτη

    // Path για το αρχείο εξαγωγής
    private string filePath;

    void Start()
    {
        filePath = Application.dataPath + "/city_map.txt";  // Ορισμός διαδρομής αρχείου
        ExportCityToFile();
    }

    void ExportCityToFile()
    {
        // Δημιουργία πίνακα για την αναπαράσταση του χάρτη
        char[,] cityMap = new char[N, M];

        // Αρχικοποίηση με κενές θέσεις
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
                cityMap[i, j] = ' ';  // Κενή θέση
            }
        }

        // Εύρεση αντικειμένων στον κόσμο μέσω tags
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        GameObject[] agentHomes = GameObject.FindGameObjectsWithTag("AgentHome");
        GameObject[] roads = GameObject.FindGameObjectsWithTag("Road");
        GameObject[] parks = GameObject.FindGameObjectsWithTag("Park");
        GameObject[] props = GameObject.FindGameObjectsWithTag("Props");
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Cars");

        // Ενημέρωση πίνακα με βάση τη θέση και τον τύπο αντικειμένου
        foreach (GameObject building in buildings)
        {
            Vector3 pos = building.transform.position;
            int x = Mathf.FloorToInt(pos.z);
            int y = Mathf.FloorToInt(pos.x);

            if (x >= 0 && x < N && y >= 0 && y < M)
            {
                cityMap[x, y] = '*';  // Ασήμαντα κτίρια
            }
        }

        foreach (GameObject home in agentHomes)
        {
            Vector3 pos = home.transform.position;
            int x = Mathf.FloorToInt(pos.z);
            int y = Mathf.FloorToInt(pos.x);

            if (x >= 0 && x < N && y >= 0 && y < M)
            {
                // Χρήση ψηφίων 0-9 για τα σπίτια πρακτόρων
                int digit = UnityEngine.Random.Range(0, 10);  // Δημιουργία τυχαίου ψηφίου
                cityMap[x, y] = (char)('0' + digit);
            }
        }

        foreach (GameObject road in roads)
        {
            Vector3 pos = road.transform.position;
            int x = Mathf.FloorToInt(pos.z);
            int y = Mathf.FloorToInt(pos.x);

            if (x >= 0 && x < N && y >= 0 && y < M)
            {
                cityMap[x, y] = 'R';  // Δρόμοι
            }
        }

        foreach (GameObject park in parks)
        {
            Vector3 pos = park.transform.position;
            int x = Mathf.FloorToInt(pos.z);
            int y = Mathf.FloorToInt(pos.x);

            if (x >= 0 && x < N && y >= 0 && y < M)
            {
                cityMap[x, y] = 'P';  // Πάρκα
            }
        }

        foreach (GameObject prop in props)
        {
            Vector3 pos = prop.transform.position;
            int x = Mathf.FloorToInt(pos.z);
            int y = Mathf.FloorToInt(pos.x);

            if (x >= 0 && x < N && y >= 0 && y < M)
            {
                cityMap[x, y] = 'X';  // Props
            }
        }

        foreach (GameObject car in cars)
        {
            Vector3 pos = car.transform.position;
            int x = Mathf.FloorToInt(pos.z);
            int y = Mathf.FloorToInt(pos.x);

            if (x >= 0 && x < N && y >= 0 && y < M)
            {
                cityMap[x, y] = 'C';  // Αυτοκίνητα
            }
        }

        // Εξαγωγή του πίνακα σε αρχείο κειμένου
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < M; j++)
                    {
                        writer.Write(cityMap[i, j]);
                    }
                    writer.WriteLine();  // Νέα γραμμή για κάθε σειρά
                }
            }
            Debug.Log("City map successfully exported to " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to export city map: " + e.Message);
        }
    }
}
