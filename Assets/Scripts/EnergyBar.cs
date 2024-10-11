using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnergyBar : MonoBehaviour
{
    Slider _energySlider;
    void Start()
    {
        _energySlider = GetComponent<Slider>();
    }

    public void SetMaxEnergy(float maxEnergy)
    {
        _energySlider.maxValue = maxEnergy;
        _energySlider.value = maxEnergy;
    }

    public void SetEnergy(float energy)
    {
        _energySlider.value = energy;
    }
}
