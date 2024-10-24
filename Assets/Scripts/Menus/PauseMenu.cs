using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

        settingsButton = document.Q<Button>("Settings");
        settingsButton.RegisterCallback<ClickEvent>(Settings);

        quitButton = document.Q<Button>("Quit");
        quitButton.RegisterCallback<ClickEvent>(Quit);
    }
    public void ActivateWindow(InputAction.CallbackContext context)
    {
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
    private void Resume(ClickEvent click)
    {
        SFXManager.Instance.PlaySFX("ButtonPress");
        GameSettings.gameState = heldState;
        background.visible = false;
    }

    private void Settings(ClickEvent click)
    {
        SFXManager.Instance.PlaySFX("ButtonPress");
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    private void Quit(ClickEvent click)
    {
        SFXManager.Instance.PlaySFX("ButtonPress");
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
