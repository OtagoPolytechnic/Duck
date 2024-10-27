using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;

public class PauseMenu : MonoBehaviour
{
    private Button resumeButton;
    private Button settingsButton;
    private Button quitButton;
    private VisualElement background;
    private GameState heldState;
    [SerializeField] private StatDisplay statDisplay;
    void Awake()
    {
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        background = document.Q<VisualElement>("Background");
        background.visible = false;

        resumeButton = document.Q<Button>("Resume");
        resumeButton.RegisterCallback<ClickEvent>(Resume);
        resumeButton.RegisterCallback<NavigationSubmitEvent>(Resume);

        settingsButton = document.Q<Button>("Settings");
        settingsButton.RegisterCallback<ClickEvent>(Settings);
        settingsButton.RegisterCallback<NavigationSubmitEvent>(Settings);

        quitButton = document.Q<Button>("Quit");
        quitButton.RegisterCallback<ClickEvent>(Quit);
        quitButton.RegisterCallback<NavigationSubmitEvent>(Quit);
    }
    public void ActivateWindow(InputAction.CallbackContext context)
    {
        //If the settings scene is open, don't allow the pause menu to close
        if (SceneManager.GetSceneByName("Settings").isLoaded)
        {
            return;
        }
        if (context.performed)
        {
            if (GameSettings.gameState == GameState.Paused)
            {
                GameSettings.gameState = heldState;
                background.visible = !background.visible; 
            }
            else if (GameSettings.gameState == GameState.InGame || GameSettings.gameState == GameState.ItemSelect)
            {
                statDisplay.UpdateStats();
                heldState = GameSettings.gameState;
                GameSettings.gameState = GameState.Paused;
                background.visible = !background.visible; 
            }
        }
    }
    private void Resume(EventBase evt)
    {
        GameSettings.gameState = heldState;
        background.visible = false;
    }

    private void Settings(EventBase evt)
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
        //Get the root element of the settings scene
        Scene settingsScene = SceneManager.GetSceneByName("Settings");

        //Makes sure the return button is focused when the settings scene is opened
        if (evt is NavigationSubmitEvent)
        {
            GameObject[] rootObjects = settingsScene.GetRootGameObjects();
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

    private void Quit(EventBase evt)
    {
        GameSettings.gameState = GameState.InGame;
        StartCoroutine(LoadScene("Titlescreen"));
    }

    IEnumerator LoadScene(string sceneName)
    {
        //TODO: Add loading screen if the load times start to get longer
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        quitButton.text = "Loading";
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
