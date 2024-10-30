using UnityEngine;
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
        paragraph.text = "Developers:\n\nAlex Reid\nKyle Black\nRohan Anakin\nPalin Wiseman\n\n" +
        "Former developers:\n\nLorna Hart\nJun Xu\n\n" +
        "Music:\n\n \"8Bit Music - 062022\"  -  GWriterStudio" +
        "Reroll icon by Stephen Kerr from Noun Project (CC BY 3.0) https://thenounproject.com/browse/icons/term/reroll/ ";
    }
    private void ReturnToMainMenu(ClickEvent click)
    {
        SFXManager.Instance.PlayRandomSFX(new string[] {"Button-Press", "Button-Press2", "Button-Press3", "Button-Press4"});
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
    }
}
