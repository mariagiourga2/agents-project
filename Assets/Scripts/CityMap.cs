using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CityMap : MonoBehaviour
{
    // Path για το αρχείο εξαγωγής
    private string filePath;

    void Start()
    {
        filePath = Application.dataPath + "/city_description.txt";  // Ορισμός διαδρομής αρχείου
        ExportCityToFile();
    }

    void ExportCityToFile()
    {
        // Δημιουργία λίστας για την περιγραφή του χάρτη
        List<string> cityDescription = new List<string>();

        // Εύρεση αντικειμένων στον κόσμο μέσω tags
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        GameObject[] agentHomes = GameObject.FindGameObjectsWithTag("AgentHome");
        GameObject[] roads = GameObject.FindGameObjectsWithTag("Road");
        GameObject[] parks = GameObject.FindGameObjectsWithTag("Park");
        GameObject[] props = GameObject.FindGameObjectsWithTag("Props");
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Cars");

        // Καταγραφή των ασήμαντων κτιρίων
        foreach (GameObject building in buildings)
        {
            Vector3 pos = building.transform.position;
            string description = $"Building at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as '*'";
            cityDescription.Add(description);
        }

        // Καταγραφή των σπιτιών πρακτόρων
        foreach (GameObject home in agentHomes)
        {
            Vector3 pos = home.transform.position;
            int digit = UnityEngine.Random.Range(0, 10);  // Χρήση ψηφίων 0-9
            string description = $"AgentHome at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as '{digit}'";
            cityDescription.Add(description);
        }

        // Καταγραφή των δρόμων
        foreach (GameObject road in roads)
        {
            Vector3 pos = road.transform.position;
            string description = $"Road at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'R'";
            cityDescription.Add(description);
        }

        // Καταγραφή των πάρκων
        foreach (GameObject park in parks)
        {
            Vector3 pos = park.transform.position;
            string description = $"Park at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'P'";
            cityDescription.Add(description);
        }

        // Καταγραφή των props
        foreach (GameObject prop in props)
        {
            Vector3 pos = prop.transform.position;
            string description = $"Prop at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'X'";
            cityDescription.Add(description);
        }

        // Καταγραφή των αυτοκινήτων
        foreach (GameObject car in cars)
        {
            Vector3 pos = car.transform.position;
            string description = $"Car at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'C'";
            cityDescription.Add(description);
        }

        // Εξαγωγή της περιγραφής σε αρχείο κειμένου
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                foreach (string line in cityDescription)
                {
                    writer.WriteLine(line);
                }
            }
            Debug.Log("City description successfully exported to " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to export city description: " + e.Message);
        }
    }
}