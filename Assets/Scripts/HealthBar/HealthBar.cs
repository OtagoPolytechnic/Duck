using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Text healthText;
    private Image healthBar;

    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    void Update()
    {
        float healthFraction = PlayerStats.Instance.CurrentHealth / PlayerStats.Instance.MaxHealth;
        healthBar.fillAmount = healthFraction;
        healthText.text = PlayerStats.Instance.CurrentHealth.ToString("F0") + "/" + PlayerStats.Instance.MaxHealth.ToString("F0");
    }
}

