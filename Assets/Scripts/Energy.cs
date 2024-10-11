using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy
{
    //fields
    float _currentEnergy;
    float _maxEnergy;
    float _energySpeed;
    bool _pauseEnergy = false;

    public float CurrentEnergy
    {
        get
        {
            return _currentEnergy;
        }
        set
        {
            _currentEnergy = value;
        }
    }

    public float MaxEnergy
    {
        get
        {
            return _maxEnergy;
        }
        set
        {
            _maxEnergy = value;
        }

    }

    public float EnergySpeed
    {
        get
        {
            return _energySpeed;
        }
        set
        {
            _energySpeed = value;
        }
    }

    public bool PauseEnergy
    {
        get
        {
            return _pauseEnergy;
        }
        set
        {
            _pauseEnergy = value;
        }
    }

    //constructor
    public Energy(float energy, float maxEnegy, float energySpeed, bool pauseEnergy)
    {
        _currentEnergy = energy;
        _maxEnergy = maxEnegy;
        _energySpeed = energySpeed;
        _pauseEnergy = pauseEnergy;
    }

    //methods
    public void UseEnergy(float energyAmount)
    {
        if (_currentEnergy > 0)
        {
            _currentEnergy -= energyAmount * Time.deltaTime;
        }

    }

    public void RegenEnergy()
    {
        if (_currentEnergy < _maxEnergy && !_pauseEnergy)
        {
            _currentEnergy += _energySpeed * Time.deltaTime;
        }

    }
}
