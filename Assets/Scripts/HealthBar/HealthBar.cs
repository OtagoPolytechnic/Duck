using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    public Label healthText;
    //private UnityEngine.UI.Image healthBar; //temp
    void Awake()
    {
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        healthText = document.Q("HealthNumber") as Label;
    }
    void Start()
    {
        //healthBar = GetComponent<UnityEngine.UI.Image>();
    }

    void Update()
    {
        float healthFraction = PlayerHealth.currentHealth / PlayerHealth.maxHealth;
        //healthBar.fillAmount = healthFraction;
        healthText.text = PlayerHealth.currentHealth.ToString("F0") + "/" + PlayerHealth.maxHealth.ToString("F0");
    }
}

