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
    private ListView moreInfoList;

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
        moreInfoList = document.Q<ListView>("MoreInfoList");
        //Set to be invisible by default
        document.style.display = DisplayStyle.None;
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
        //Clear selected items
        highscores.ClearSelection();
        clearMoreInfoList();
    }

    public void DisplayBossHighscores(ClickEvent click = null) //Nullable so I can call it manually as well
    {
        if (displayedHighscores == HighscoreType.Boss) //If the boss data is already displayed then don't display it again
        {
            return;
        }
        displayedHighscores = HighscoreType.Boss;
        highscores.ClearSelection();
        clearMoreInfoList();
        DisplayHighscores(Scoreboard.Instance.bossSavedScores.highscores);
        //Clear selected items
    }

    //Making MultiColumnListViews are really confusing. This is the result of several hours of trial and error.
    //I used the docs, forum posts, copilot, and chatgpt
    private void DisplayHighscores(List<EntryData> highscoreData)
    {
        //Clear the previous data
        highscores.itemsSource = new List<EntryData>();
        highscores.Rebuild();

        //If the list is empty return
        if (highscoreData == null || highscoreData.Count == 0 || highscoreData[0] == null)
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
        highscores.selectionChanged += OnRowClicked;
    }

    //TODO: Get to show a window
    private void OnRowClicked(IEnumerable<object> selectedItems)
    {
        foreach (EntryData item in selectedItems)
        {
            List<string> info = item.GetEntryInfo();
            moreInfoList.itemsSource = info;
            moreInfoList.Rebuild();
        }
    }

    private void clearMoreInfoList()
    {
        //Make the list just have an empty string so there is not list empty message
        moreInfoList.itemsSource = new List<string> { "" };
        moreInfoList.Rebuild();
    }
}

