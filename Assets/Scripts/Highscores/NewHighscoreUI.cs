using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class NewHighscoreUI : MonoBehaviour
{
    private VisualElement document;

    void Awake()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
        Button menuButton = document.Q<Button>("MainMenu");
        menuButton.RegisterCallback<ClickEvent>(Menu);
    }

    private void Menu(ClickEvent click)
    {
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
    }
 
}   

