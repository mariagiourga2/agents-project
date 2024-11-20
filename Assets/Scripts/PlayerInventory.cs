using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfDiamonds { get; private set; }
    public UnityEvent<PlayerInventory> OnDiamondCollected;
    public UnityEvent<PlayerInventory> OnHeartCollected;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    [SerializeField] private TextMeshProUGUI TextE;
    private float timer = 0f;
    private float timeInterval = 1f;

    public void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHeath(maxHealth);
        TextE = GameObject.Find("TextE")
            .GetComponent<TextMeshProUGUI>();
        if (TextE == null)
        {
            Debug.LogError("TextE not found! Check your object name or assignment.");
        }
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeInterval)
        {
            timer = 0f;
            TakeDamage(1);
        }
        else if(TextE != null)
{
            TextE.text = currentHealth.ToString();
        }
    }
   
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHeath(currentHealth);
    }
    public void HeartCollected()
    {
        if (currentHealth + 20 < maxHealth)
        {
            currentHealth = currentHealth + 20;
            healthBar.SetHeath(currentHealth);
            TextE.text = Mathf.FloorToInt(currentHealth).ToString();
            Debug.Log(currentHealth.ToString());
            OnHeartCollected.Invoke(this);
        }
        else if(currentHealth + 20 >= maxHealth) 
        {
            currentHealth = maxHealth;
            healthBar.SetHeath(currentHealth);
            TextE.text = Mathf.FloorToInt(currentHealth).ToString();
            Debug.Log(currentHealth.ToString());
            OnHeartCollected.Invoke(this);
        }
    }

    public void DiamondCollected()
    {
        NumberOfDiamonds++;
        OnDiamondCollected.Invoke(this);
    }
}
