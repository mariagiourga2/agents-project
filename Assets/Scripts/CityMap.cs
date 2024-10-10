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
        GameObject[] bakery = GameObject.FindGameObjectsWithTag("B");
        GameObject[] superMarket = GameObject.FindGameObjectsWithTag("M");
        GameObject[] stadium = GameObject.FindGameObjectsWithTag("S");
        GameObject[] drugStore = GameObject.FindGameObjectsWithTag("D");
        GameObject[] gasStation = GameObject.FindGameObjectsWithTag("G");
        GameObject[] facrory = GameObject.FindGameObjectsWithTag("F");
        GameObject[] agentHome1 = GameObject.FindGameObjectsWithTag("Home1");
        GameObject[] agentHome2 = GameObject.FindGameObjectsWithTag("Home2");
        GameObject[] agentHome3 = GameObject.FindGameObjectsWithTag("Home3");
        GameObject[] agentHome4 = GameObject.FindGameObjectsWithTag("Home4");
        GameObject[] agentHome5 = GameObject.FindGameObjectsWithTag("Home5");
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
        // Καταγραφή των σημαντικών κτιρίων
        //Φούρνος
        foreach (GameObject building in bakery)
        {
            Vector3 pos = building.transform.position;
            string description = $"Bakery at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'B'";
            cityDescription.Add(description);
        }
        //Super Market
        foreach (GameObject building in superMarket)
        {
            Vector3 pos = building.transform.position;
            string description = $"Super Market at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'M'";
            cityDescription.Add(description);
        }
        //Στάδιο
        foreach (GameObject building in stadium)
        {
            Vector3 pos = building.transform.position;
            string description = $"Stadium at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'S'";
            cityDescription.Add(description);
        }
        //Φαρμακείο
        foreach (GameObject building in drugStore)
        {
            Vector3 pos = building.transform.position;
            string description = $"Drug Store at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'D'";
            cityDescription.Add(description);
        }
        //Βενζινάδικο
        foreach (GameObject building in gasStation)
        {
            Vector3 pos = building.transform.position;
            string description = $"Gas Station at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'G'";
            cityDescription.Add(description);
        }
        //Εργοστάσιο
        foreach (GameObject building in facrory)
        {
            Vector3 pos = building.transform.position;
            string description = $"Factory at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'F'";
            cityDescription.Add(description);
        }

        // Καταγραφή των σπιτιών πρακτόρων
        foreach (GameObject home in agentHome1)
        {
            Vector3 pos = home.transform.position;
            string description = $"AgentHome1 at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as '1'";
            cityDescription.Add(description);
        }

        foreach (GameObject home in agentHome2)
        {
            Vector3 pos = home.transform.position;
            string description = $"AgentHome2 at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as '2'";
            cityDescription.Add(description);
        }

        foreach (GameObject home in agentHome3)
        {
            Vector3 pos = home.transform.position;
            string description = $"AgentHome3 at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as '3'";
            cityDescription.Add(description);
        }

        foreach (GameObject home in agentHome4)
        {
            Vector3 pos = home.transform.position;
            string description = $"AgentHome4 at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as '4'";
            cityDescription.Add(description);
        }

        foreach (GameObject home in agentHome5)
        {
            Vector3 pos = home.transform.position;
            string description = $"AgentHome5 at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as '5'";
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