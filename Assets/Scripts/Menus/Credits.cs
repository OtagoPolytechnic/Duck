using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Credits : MonoBehaviour
{
    public VisualElement document;
    void Awake()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
        Button goBack = document.Q<Button>("Return");
        goBack.RegisterCallback<ClickEvent>(ReturnToMainMenu);

        Label paragraph = document.Q<Label>("Paragraph");
        paragraph.text = "Developers:\n\nAlex Reid\nKyle Black\nRohan Anakin\nPalin Wiseman\n\nFormer developers:\n\nLorna Hart\nJun Xu";
    }
    private void ReturnToMainMenu(ClickEvent click)
    {
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
    }
}
