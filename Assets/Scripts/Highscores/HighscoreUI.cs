using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HighscoreUI : MonoBehaviour
{
    private VisualElement document;
    private MultiColumnListView highscores;
    public static HighscoreUI Instance; //Singleton pattern
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        document = GetComponent<UIDocument>().rootVisualElement;
        Button menuButton = document.Q<Button>("MainMenu");
        menuButton.RegisterCallback<ClickEvent>(Menu);
        //Get the highscores list view
        highscores = document.Q<MultiColumnListView>("MultiColumnListView");
    }

    private void Menu(ClickEvent click)
    {
        if (document != null)
        {
            if (SceneManager.GetSceneByName("Titlescreen").isLoaded) //If coming from the main menu
            {
                document.style.display = DisplayStyle.None;
            }
            else //If coming from in game
            {
                SceneManager.UnloadSceneAsync("Highscores");
            }
        }
    }

    //Making MultiColumnListViews are really confusing. Used copilot to help me understand it
    public void DisplayHighscores(List<EntryData> highscoreData)
    {
        // Set the itemsSource to populate the data in the list.
        highscores.itemsSource = highscoreData;



        // Define the columns and their respective makeCell and bindCell methods.
        highscores.columns["date"].makeCell = () => new Label();
        highscores.columns["name"].makeCell = () => new Label();
        highscores.columns["score"].makeCell = () => new Label();
        highscores.columns["weapon"].makeCell = () => new Label();

        highscores.columns["date"].bindCell = (VisualElement element, int index) =>
            (element as Label).text = highscoreData[index].GetDate();
        highscores.columns["name"].bindCell = (VisualElement element, int index) =>
            (element as Label).text = highscoreData[index].entryName;
        highscores.columns["score"].bindCell = (VisualElement element, int index) =>
            (element as Label).text = highscoreData[index].entryScore.ToString();
        highscores.columns["weapon"].bindCell = (VisualElement element, int index) =>
            (element as Label).text = highscoreData[index].weapon.ToString();

        highscores.Rebuild();
    }
}

