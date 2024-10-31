using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.InputSystem;

public class ModeSelect : MonoBehaviour
{
    private Button bossPlayButton;
    private Button endlessPlayButton;
    private Button backButton;
    private VisualElement document;
    // Start is called before the first frame update
    void Start()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
        bossPlayButton = document.Q<Button>("BossPlayGame");
        endlessPlayButton = document.Q<Button>("EndlessPlayGame");
        backButton = document.Q<Button>("Return");
        bossPlayButton.RegisterCallback<ClickEvent>(BossPlayGame);
        bossPlayButton.RegisterCallback<NavigationSubmitEvent>(BossPlayGame);
        endlessPlayButton.RegisterCallback<ClickEvent>(EndlessPlayGame);
        endlessPlayButton.RegisterCallback<NavigationSubmitEvent>(EndlessPlayGame);
        backButton.RegisterCallback<ClickEvent>(ReturnToMainMenu);
        backButton.RegisterCallback<NavigationSubmitEvent>(ReturnToMainMenu);
        navigationSetting();
    }

    private void BossPlayGame(EventBase evt)
    {
        SFXManager.Instance.PlayRandomSFX(new string[] {"Button-Press", "Button-Press2", "Button-Press3", "Button-Press4"});
        GameSettings.gameMode = GameMode.Boss;
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
        showSkillMenu();
    }

    private void EndlessPlayGame(EventBase evt)
    {
        SFXManager.Instance.PlayRandomSFX(new string[] {"Button-Press", "Button-Press2", "Button-Press3", "Button-Press4"});
        GameSettings.gameMode = GameMode.Endless;
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
        showSkillMenu();
    }

    private void navigationSetting()
    {
        bossPlayButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch(e.direction)
            {
                case NavigationMoveEvent.Direction.Up: backButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down: backButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: endlessPlayButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: endlessPlayButton.Focus(); break;
            }
            e.PreventDefault();
        });

        endlessPlayButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch(e.direction)
            {
                case NavigationMoveEvent.Direction.Up: backButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down: backButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: bossPlayButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: bossPlayButton.Focus(); break;
            }
            e.PreventDefault();
        });

        backButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch(e.direction)
            {
                case NavigationMoveEvent.Direction.Up: bossPlayButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down: bossPlayButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: endlessPlayButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: bossPlayButton.Focus(); break;
            }
            e.PreventDefault();
        });
    }

    //Copying from Menu.cs
    private void showSkillMenu()
    {
        Scene skillScene = SceneManager.GetSceneByName("SkillMenu");
        if (skillScene.IsValid())
        {
            GameObject[] rootObjects = skillScene.GetRootGameObjects(); //Gets an array of all the objects in the scene that aren't inside other objects
            UIDocument uiDocument = rootObjects
                .Select(obj => obj.GetComponent<UIDocument>())
                .FirstOrDefault(doc => doc != null); //Checking each object to see if it has a UIDocument component, and if it does, it returns it
            
            if (uiDocument != null)
            {
                VisualElement rootElement = uiDocument.rootVisualElement; //Getting the root visual element of the UI document
                rootElement.style.display = DisplayStyle.Flex;
                Button buttonToFocus = rootElement.Query<Button>(className: "focus-button").First();
                if (buttonToFocus != null)
                {
                    buttonToFocus.Focus();
                }
            }
        }
    }

    private void ReturnToMainMenu(EventBase evt)
    {
        SFXManager.Instance.PlayRandomSFX(new string[] {"Button-Press", "Button-Press2", "Button-Press3", "Button-Press4"});
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
        Scene Titlescreen = SceneManager.GetSceneByName("Titlescreen");
        //Makes sure the return button is focused when the settings scene is opened
        if (evt is NavigationSubmitEvent)
        {
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
