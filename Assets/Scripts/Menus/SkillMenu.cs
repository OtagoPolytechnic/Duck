using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SkillMenu : MonoBehaviour
{

    private Button playButton;

    void Awake()
    {
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        playButton = document.Q<Button>("Play");
        playButton.RegisterCallback<ClickEvent>(Play);

    }
    public void Play(ClickEvent click)
    {
        GameSettings.gameState = GameState.InGame;
        StartCoroutine(LoadScene("MainScene"));
    }

    IEnumerator LoadScene(string sceneName)
    {
        //TODO: Add loading screen if the load times start to get longer
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        playButton.text = "Loading";
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
