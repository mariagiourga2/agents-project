using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CityMap : MonoBehaviour
{
    // ��������� ��� ��� �����
    public int N = 300;  // ���� ��� �����
    public int M = 260;  // ������ ��� �����

    // Path ��� �� ������ ��������
    private string filePath;

    void Start()
    {
        filePath = Application.dataPath + "/city_map.txt";  // ������� ��������� �������
        ExportCityToFile();
    }

    void ExportCityToFile()
    {
        // ���������� ������ ��� ��� ������������ ��� �����
        char[,] cityMap = new char[N, M];

        // ������������ �� ����� ������
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
                cityMap[i, j] = ' ';  // ���� ����
            }
        }

        // ������ ������������ ���� ����� ���� tags
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        GameObject[] agentHomes = GameObject.FindGameObjectsWithTag("AgentHome");
        GameObject[] roads = GameObject.FindGameObjectsWithTag("Road");
        GameObject[] parks = GameObject.FindGameObjectsWithTag("Park");
        GameObject[] props = GameObject.FindGameObjectsWithTag("Props");
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Cars");

        // ��������� ������ �� ���� �� ���� ��� ��� ���� ������������
        foreach (GameObject building in buildings)
        {
            Vector3 pos = building.transform.position;
            int x = Mathf.FloorToInt(pos.z);
            int y = Mathf.FloorToInt(pos.x);

            if (x >= 0 && x < N && y >= 0 && y < M)
            {
                cityMap[x, y] = '*';  // �������� ������
            }
        }

        foreach (GameObject home in agentHomes)
        {
            Vector3 pos = home.transform.position;
            int x = Mathf.FloorToInt(pos.z);
            int y = Mathf.FloorToInt(pos.x);

            if (x >= 0 && x < N && y >= 0 && y < M)
            {
                // ����� ������ 0-9 ��� �� ������ ���������
                int digit = UnityEngine.Random.Range(0, 10);  // ���������� ������� ������
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
                cityMap[x, y] = 'R';  // ������
            }
        }

        foreach (GameObject park in parks)
        {
            Vector3 pos = park.transform.position;
            int x = Mathf.FloorToInt(pos.z);
            int y = Mathf.FloorToInt(pos.x);

            if (x >= 0 && x < N && y >= 0 && y < M)
            {
                cityMap[x, y] = 'P';  // �����
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
                cityMap[x, y] = 'C';  // ����������
            }
        }

        // ������� ��� ������ �� ������ ��������
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
                    writer.WriteLine();  // ��� ������ ��� ���� �����
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
