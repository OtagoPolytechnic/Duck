using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    void Awake()
    {
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        Button goBack = document.Q<Button>("Return");
        goBack.RegisterCallback<ClickEvent>(ReturnToMainMenu);

        Label paragraph = document.Q<Label>("Paragraph");
        paragraph.text = "WASD to move. Hold left click to shoot. after each wave you get a choice of item. see how long you can survive";
    }
    private void ReturnToMainMenu(ClickEvent click)
    {
        SceneManager.LoadScene("Titlescreen");
    }
}
