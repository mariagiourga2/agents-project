using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI diamondText;
    private TextMeshProUGUI TextE;
    

    void Start()
    {
        diamondText = GetComponent<TextMeshProUGUI>();
        TextE = GetComponent<TextMeshProUGUI>();
        
    }

    public void UpdateDiamondText(PlayerInventory playerInventory)
    {
        diamondText.text = playerInventory.NumberOfDiamonds.ToString();
        
    }
    public void UpdateHeartText(PlayerInventory playerInventory)
    {
        TextE.text = playerInventory.currentHealth.ToString();
        
    }
}

