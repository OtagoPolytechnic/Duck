using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;

public class Credits : MonoBehaviour
{
    public VisualElement document;
    void Awake()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
        Button goBack = document.Q<Button>("Return");
        goBack.RegisterCallback<ClickEvent>(ReturnToMainMenu);
        goBack.RegisterCallback<NavigationSubmitEvent>(ReturnToMainMenu);

        Label paragraph = document.Q<Label>("Paragraph");
        paragraph.text = "Developers:\n\nAlex Reid\nKyle Black\nRohan Anakin\nPalin Wiseman\n\n" +
        "Former developers:\n\nLorna Hart\nJun Xu\n\n" +
        "Music:\n\n \"8Bit Music - 062022\"  -  GWriterStudio" +
        "Reroll icon by Stephen Kerr from Noun Project (CC BY 3.0) https://thenounproject.com/browse/icons/term/reroll/ ";
    }

    void Start()
    {
        navigationSetting();
    }

    private void navigationSetting()
    {
        Button goBack = document.Q<Button>("Return");
        goBack.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: goBack.Focus(); break;
                case NavigationMoveEvent.Direction.Down: goBack.Focus(); break;
                case NavigationMoveEvent.Direction.Left: goBack.Focus(); break;
                case NavigationMoveEvent.Direction.Right: goBack.Focus(); break;
            }
            e.PreventDefault();
        });
    }

    private void ReturnToMainMenu(EventBase evt)
    {
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
            if (evt is NavigationSubmitEvent)
            {
                Scene Titlescreen = SceneManager.GetSceneByName("Titlescreen");
                GameObject[] rootObjects = Titlescreen.GetRootGameObjects();
                UIDocument uiDocument = rootObjects
                    .Select(obj => obj.GetComponent<UIDocument>())
                    .FirstOrDefault(doc => doc != null);
                if (uiDocument != null)
                {
                    VisualElement rootElement = uiDocument.rootVisualElement;
                    Button buttonToFocus = rootElement.Query<Button>(className: "focus-button").First();
                    if (buttonToFocus != null)
                    {
                        buttonToFocus.Focus();
                    }
                }
            }
        }
    }
}
