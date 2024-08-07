using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private Label healthText;
    private IMGUIContainer healthBar;
    private float maxHealthBarSize;
    
    void Awake()
    {
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        healthText = document.Q<Label>("HealthNumber");
        healthBar = document.Q<IMGUIContainer>("Health");
        maxHealthBarSize = healthBar.style.width.value.value;  //https://discussions.unity.com/t/parse-int-to-stylelength/880675/3
    }

    void Update()
    {
        float healthFraction = PlayerStats.Instance.CurrentHealth / PlayerStats.Instance.MaxHealth;
        healthBar.style.width = Length.Percent(healthFraction * 100);
        healthText.text = PlayerStats.Instance.CurrentHealth.ToString("F0") + "/" + PlayerStats.Instance.MaxHealth.ToString("F0");
    }
}