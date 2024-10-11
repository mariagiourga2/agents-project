using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehaviour : MonoBehaviour
{
    [SerializeField] EnergyBar _energybar;
    [SerializeField] AgentController _agentController;
    float _agentOriginalSpeed;
    float _agentSprintSpeed;
    void Start()
    {
        _agentOriginalSpeed = _agentController._speed;
        _agentSprintSpeed = _agentController._speed * 2f;
    }

    void Update()
    {
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (GameManager.gameManager._agentEnergy.CurrentEnergy > 0)
            {
                AgentUseEnergy(60f);
                if(_agentController._speed != _agentSprintSpeed)
                {
                    _agentController._speed = _agentSprintSpeed;
                }
            }
            else
            {
                _agentController._speed = _agentOriginalSpeed;
            }
        }
        /*else
        {
            AgentRegenEnergy();
            if (_agentController._speed != _agentOriginalSpeed)
            {
                _agentController._speed = _agentOriginalSpeed;
            }

        }*/
    }
    private void AgentUseEnergy(float enegyAmount)
    {
        GameManager.gameManager._agentEnergy.UseEnergy(enegyAmount);
        _energybar.SetEnergy(GameManager.gameManager._agentEnergy.CurrentEnergy);
    }

  /*  private void AgentRegenEnergy()
    {
        GameManager.gameManager._agentEnergy.RegenEnergy();
        _energybar.SetEnergy(GameManager.gameManager._agentEnergy.CurrentEnergy);
    }*/
}
