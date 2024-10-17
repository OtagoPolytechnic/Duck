using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance;
    private Label healthText;
    private IMGUIContainer healthBar;
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        healthText = document.Q<Label>("HealthNumber");
        healthBar = document.Q<IMGUIContainer>("Health");
      
    }
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        float healthFraction = (float)currentHealth / maxHealth; //As these are both ints now I need to cast one to a float
        healthBar.style.width = Length.Percent(healthFraction * 100);
        healthText.text = currentHealth.ToString("F0") + "/" + maxHealth.ToString("F0");
    }
}