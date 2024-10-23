using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;

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
        endlessPlayButton.RegisterCallback<ClickEvent>(EndlessPlayGame);
        backButton.RegisterCallback<ClickEvent>(ReturnToMainMenu);
    }

    private void BossPlayGame(ClickEvent click)
    {
        SFXManager.Instance.PlaySFX("ButtonPress");
        GameSettings.gameMode = GameMode.Boss;
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
        showSkillMenu();
    }

    private void EndlessPlayGame(ClickEvent click)
    {
        SFXManager.Instance.PlaySFX("ButtonPress");
        GameSettings.gameMode = GameMode.Endless;
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
        showSkillMenu();
    }

    //Copying from Menu.cs
    private void showSkillMenu()
    {
        Scene skillScene = SceneManager.GetSceneByName("SkillMenu");
        if (skillScene.IsValid())
        {
            GameObject[] rootObjects = skillScene.GetRootGameObjects(); // Gets an array of all the objects in the scene that aren't inside other objects
            UIDocument uiDocument = rootObjects
                .Select(obj => obj.GetComponent<UIDocument>())
                .FirstOrDefault(doc => doc != null); // Checking each object to see if it has a UIDocument component, and if it does, it returns it
            
            if (uiDocument != null)
            {
                VisualElement rootElement = uiDocument.rootVisualElement; // Getting the root visual element of the UI document
                rootElement.style.display = DisplayStyle.Flex;
            }
        }
    }

    private void ReturnToMainMenu(ClickEvent click)
    {
        SFXManager.Instance.PlaySFX("ButtonPress");
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
    }
}
