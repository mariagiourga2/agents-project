using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI TextE;
    [SerializeField] float energy = 100f;
    [SerializeField] float remainingTime;
    public float RemainingTime => remainingTime;

    void Update()
    {

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            energy = Mathf.Clamp(energy - Time.deltaTime * 1, 0, 100);
        }

        else if (remainingTime < 0)
        {
            remainingTime = 0;
            //GameOver;
            timerText.color = Color.red;
            SceneManager.LoadSceneAsync(0);
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        Debug.Log(energy.ToString());
        TextE.text = Mathf.FloorToInt(energy).ToString();
    }
}
