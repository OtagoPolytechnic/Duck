using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance;
    private Label healthText;
    private IMGUIContainer healthBar;
    private VisualElement[] respawnIcons;
    private VisualElement emptyRespawnIcon;
    private Label respawnText;
    
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
        respawnText = document.Q<Label>("LifeText");
        emptyRespawnIcon = document.Q<VisualElement>("Empty_Heart0");
        respawnIcons = new VisualElement[3];
        for (int i = 0; i < 3; i++)
        {
            respawnIcons[i] = document.Q<VisualElement>("Heart" + i);
        }
      
    }
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        float healthFraction = (float)currentHealth / maxHealth; //As these are both ints now I need to cast one to a float
        healthBar.style.width = Length.Percent(healthFraction * 100);
        healthText.text = currentHealth.ToString("F0") + "/" + maxHealth.ToString("F0");
    }

    public void UpdateRespawnDisplay(int respawns)
    {
        if (respawns > 3)
        {
            if (respawns > 9)
            {
                respawnText.text = "9+";
            }
            else
            {
                respawnText.text = respawns.ToString();
            }
            emptyRespawnIcon.style.display = DisplayStyle.None;
            respawnIcons[0].style.display = DisplayStyle.Flex;
            respawnIcons[1].style.display = DisplayStyle.None;
            respawnIcons[2].style.display = DisplayStyle.None;
        }
        else
        {
            respawnText.text = "";
            if (respawns == 0)
            {
                emptyRespawnIcon.style.display = DisplayStyle.Flex;
            }
            else
            {
                emptyRespawnIcon.style.display = DisplayStyle.None;
            }
            for (int i = 0; i < 3; i++)
            {
                if (i < respawns)
                {
                    respawnIcons[i].style.display = DisplayStyle.Flex;
                }
                else
                {
                    respawnIcons[i].style.display = DisplayStyle.None;
                }
            }
        }
    }
}