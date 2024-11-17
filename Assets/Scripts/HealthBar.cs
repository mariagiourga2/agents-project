using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public void SetMaxHeath(int heath)
    {
        slider.maxValue = heath;
        slider.value = heath;  
    }
    public void SetHeath(int heath)
    {
        slider.value = heath;
    }
}
