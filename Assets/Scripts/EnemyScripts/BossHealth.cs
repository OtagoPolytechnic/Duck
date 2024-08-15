using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UIElements;

public class BossHealth : MonoBehaviour
{
    public static BossHealth Instance;
    private Label healthText;
    private IMGUIContainer healthBar;
    private float maxHealthBarSize;
    public EnemyHealth boss;
    private float bossMaxHealth;

    public float BossMaxHealth
    { 
        get { return bossMaxHealth;}
        set { bossMaxHealth = value; }
    }

    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        healthText = document.Q<Label>("HealthNumber");
        healthBar = document.Q<IMGUIContainer>("Health");
    }

    void Update()
    {
        float healthFraction = boss.health / bossMaxHealth;
        healthBar.style.width = Length.Percent(healthFraction * 100);
        healthText.text = boss.health.ToString("F0") + "/" + bossMaxHealth.ToString("F0");
    }
}