using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements.Experimental;
using System;

public class HighscoreUI : MonoBehaviour
{
    private enum HighscoreType //This is so I can keep track of which highscores are being displayed and not rewrite the same data
    {
        Boss,
        Endless,
        None
    }
    private VisualElement document;
    private MultiColumnListView highscores;
    public static HighscoreUI Instance; //Singleton pattern
    private Button bossButton;
    private Button endlessButton;
    private HighscoreType displayedHighscores = HighscoreType.None;
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
        bossButton = document.Q<Button>("Boss");
        endlessButton = document.Q<Button>("Endless");
        menuButton.RegisterCallback<ClickEvent>(Menu);
        bossButton.RegisterCallback<ClickEvent>(DisplayBossHighscores);
        endlessButton.RegisterCallback<ClickEvent>(DisplayEndlessHighscores);
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

    public void DisplayEndlessHighscores(ClickEvent click = null) //Nullable so I can call it manually as well
    {
        if (displayedHighscores == HighscoreType.Endless) //If the endless data is already displayed then don't display it again
        {
            return;
        }
        displayedHighscores = HighscoreType.Endless;
        DisplayHighscores(Scoreboard.Instance.endlessSavedScores.highscores);
    }

    public void DisplayBossHighscores(ClickEvent click = null) //Nullable so I can call it manually as well
    {
        if (displayedHighscores == HighscoreType.Boss) //If the boss data is already displayed then don't display it again
        {
            return;
        }
        displayedHighscores = HighscoreType.Boss;
        DisplayHighscores(Scoreboard.Instance.bossSavedScores.highscores);
    }

    //Making MultiColumnListViews are really confusing. This is the result of several hours of trial and error.
    //I used the docs, forum posts, copilot, and chatgpt
    private void DisplayHighscores(List<EntryData> highscoreData)
    {
        //Clear the previous data
        highscores.itemsSource = new List<EntryData>();
        highscores.Rebuild();

        //If the list is empty return
        if (highscoreData == null || highscoreData.Count == 0)
        {
            return;
        }

        //Set the new data
        highscores.itemsSource = highscoreData;

        // Helper method to bind cell data
        void BindCell(VisualElement element, int index, Func<EntryData, string> getValue)
        {
            if (index >= 0 && index < highscoreData.Count)
            {
                (element as Label).text = getValue(highscoreData[index]);
            }
        }

        //Define how to create cells and bind data
        highscores.columns["date"].makeCell = () => new Label();
        highscores.columns["name"].makeCell = () => new Label();
        highscores.columns["score"].makeCell = () => new Label();
        highscores.columns["weapon"].makeCell = () => new Label();

        highscores.columns["date"].bindCell = (element, index) => BindCell(element, index, data => data.DateFormatted);
        highscores.columns["name"].bindCell = (element, index) => BindCell(element, index, data => data.entryName);
        highscores.columns["score"].bindCell = (element, index) => BindCell(element, index, data => data.entryScore.ToString());
        highscores.columns["weapon"].bindCell = (element, index) => BindCell(element, index, data => data.weapon.ToString());

        //Rebuild the list view
        highscores.Rebuild();

        // Set up click event handler
        highscores.onSelectionChange += OnRowClicked;
        //This is double click. Work out this later
        //highscores.onItemsChosen += OnRowClicked;
    }

    private void OnRowClicked(IEnumerable<object> selectedItems)
    {
        foreach (var item in selectedItems)
        {
            if (item is EntryData entryData)
            {
                // Display more data for the selected entry
                Debug.Log($"Name: {entryData.entryName}, Score: {entryData.entryScore}, Weapon: {entryData.weapon}, Wave: {entryData.waveNumber}, Enemies Killed: {entryData.enemiesKilled}");
            }
        }
    }
}

