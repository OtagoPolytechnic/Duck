using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    public VisualElement document;
    void Awake()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
        Button goBack = document.Q<Button>("Return");
        goBack.RegisterCallback<ClickEvent>(ReturnToMainMenu);

        Label paragraph = document.Q<Label>("Paragraph");
        paragraph.text = "WASD to move. Hold left click to shoot.\nAfter each wave you get a choice of item.\nSee how long you can survive!";
    }
    private void ReturnToMainMenu(ClickEvent click)
    {
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
    }
}
