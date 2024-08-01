using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private Label healthText;
    private IMGUIContainer healthBar;
    private float maxHealthBarSize;
    //private UnityEngine.UI.Image healthBar; //temp
    void Awake()
    {
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        healthText = document.Q<Label>("HealthNumber");
        healthBar = document.Q<IMGUIContainer>("Health");
        maxHealthBarSize = healthBar.style.width.value.value;  //https://discussions.unity.com/t/parse-int-to-stylelength/880675/3
    }

    void Update()
    {
        float healthFraction =  PlayerHealth.currentHealth / PlayerHealth.maxHealth;
        healthBar.style.width = Length.Percent(healthFraction * 100);
        healthText.text = PlayerHealth.currentHealth.ToString("F0") + "/" + PlayerHealth.maxHealth.ToString("F0");
    }
}

