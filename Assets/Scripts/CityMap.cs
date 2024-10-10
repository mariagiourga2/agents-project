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
        GameObject[] agentHomes = GameObject.FindGameObjectsWithTag("AgentHome");
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

        // ��������� ��� ������� ���������
        foreach (GameObject home in agentHomes)
        {
            Vector3 pos = home.transform.position;
            int digit = UnityEngine.Random.Range(0, 10);  // ����� ������ 0-9
            string description = $"AgentHome at position (x: {pos.x}, y: {pos.y}, z: {pos.z}) marked as '{digit}'";
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