using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UIElements;

public class BossHealthBar : MonoBehaviour
{
    public static BossHealthBar Instance;
    private Label healthText;
    private IMGUIContainer healthBar;
    private VisualElement healthContainer; //Reference to the container holding the health bar
    private float maxHealthBarSize;
    public EnemyBase boss;
    private float bossMaxHealth;
    private VisualElement shieldIcon;
    private Label shieldText;
    public bool finalBossShield;

    public float BossMaxHealth
    { 
        get { return bossMaxHealth; }
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
        healthContainer = document.Q<VisualElement>("BossHealthContainer"); // Reference to the container
        shieldIcon = document.Q<VisualElement>("ShieldIcon");
        shieldText = document.Q<Label>("ShieldNumber");
    }

    void Update()
    {
        if (boss == null)
        {
            return;
        }

        float healthFraction = boss.Health / bossMaxHealth;
        healthBar.style.width = Length.Percent(healthFraction * 100);
        healthText.text = boss.Health.ToString("F0") + "/" + bossMaxHealth.ToString("F0");
        if (RiotShield.Instance != null && RiotShield.Instance.shieldHealth > 0)
        {
            shieldIcon.visible = true;
            shieldText.text = RiotShield.Instance.shieldHealth.ToString();
        }
        else if (finalBossShield)
        {
            shieldIcon.visible = true;
            shieldText.text = "∞";
        }
        else if (shieldIcon.visible)
        {
            shieldIcon.visible = false;
        }

        // Hide the health bar if boss health is 0 or less
        if (boss.Health <= 0)
        {
            healthContainer.visible = false; // Hide the container holding the health bar
            shieldIcon.visible = false;
        }
    }
}
