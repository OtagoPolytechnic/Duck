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
        float healthFraction = PlayerHealth.Instance.CurrentHealth / PlayerHealth.Instance.MaxHealth;
        healthBar.fillAmount = healthFraction;
        healthText.text = PlayerHealth.Instance.CurrentHealth.ToString("F0") + "/" + PlayerHealth.Instance.MaxHealth.ToString("F0");
    }
}

