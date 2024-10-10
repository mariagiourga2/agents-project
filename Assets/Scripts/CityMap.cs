using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CityMap : MonoBehaviour
{
    // Path ��� �� ������ ��������
    private string filePath;

    void Start()
    {
        filePath = Application.dataPath + "/city_description.txt";  // ������� ��������� �������
        ExportCityToFile();
    }

    void ExportCityToFile()
    {
        // ���������� ������ ��� ��� ��������� ��� �����
        List<string> cityDescription = new List<string>();

        // ������ ������������ ���� ����� ���� tags
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

        // ��������� ��� ��������� �������
        foreach (GameObject building in buildings)
        {
            Vector3 pos = building.transform.position;
            string description = $"Building at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as '*'";
            cityDescription.Add(description);
        }
        // ��������� ��� ���������� �������
        //�������
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
        //������
        foreach (GameObject building in stadium)
        {
            Vector3 pos = building.transform.position;
            string description = $"Stadium at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'S'";
            cityDescription.Add(description);
        }
        //���������
        foreach (GameObject building in drugStore)
        {
            Vector3 pos = building.transform.position;
            string description = $"Drug Store at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'D'";
            cityDescription.Add(description);
        }
        //�����������
        foreach (GameObject building in gasStation)
        {
            Vector3 pos = building.transform.position;
            string description = $"Gas Station at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'G'";
            cityDescription.Add(description);
        }
        //����������
        foreach (GameObject building in facrory)
        {
            Vector3 pos = building.transform.position;
            string description = $"Factory at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'F'";
            cityDescription.Add(description);
        }

        // ��������� ��� ������� ���������
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

        // ��������� ��� ������
        foreach (GameObject road in roads)
        {
            Vector3 pos = road.transform.position;
            string description = $"Road at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'R'";
            cityDescription.Add(description);
        }

        // ��������� ��� ������
        foreach (GameObject park in parks)
        {
            Vector3 pos = park.transform.position;
            string description = $"Park at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'P'";
            cityDescription.Add(description);
        }

        // ��������� ��� props
        foreach (GameObject prop in props)
        {
            Vector3 pos = prop.transform.position;
            string description = $"Prop at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'X'";
            cityDescription.Add(description);
        }

        // ��������� ��� �����������
        foreach (GameObject car in cars)
        {
            Vector3 pos = car.transform.position;
            string description = $"Car at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as 'C'";
            cityDescription.Add(description);
        }

        // ������� ��� ���������� �� ������ ��������
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