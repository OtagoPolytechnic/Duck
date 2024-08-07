using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class NewHighscoreUI : MonoBehaviour
{
    void Awake()
    {
        VisualElement highscoreUI = GetComponent<UIDocument>().rootVisualElement;
        Button menuButton = highscoreUI.Q<Button>("MainMenu");
        menuButton.RegisterCallback<ClickEvent>(Menu);
    }

    private void Menu(ClickEvent click)
    {
        SceneManager.LoadScene("Titlescreen");
    }
 
}   

