using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements.Experimental;
using System;
using UnityEngine.InputSystem;

public class HighscoreUI : MonoBehaviour
{
    private VisualElement document;
    private MultiColumnListView highscores;
    public static HighscoreUI Instance; //Singleton pattern
    private Button bossButton;
    private Button endlessButton;
    private GameMode displayedHighscores = GameMode.None;
    private ListView moreInfoList;
    private Color bossColour;
    private Color endlessColour;
    private Button menuButton;

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
        menuButton = document.Q<Button>("MainMenu");
        bossButton = document.Q<Button>("Boss");
        endlessButton = document.Q<Button>("Endless");
        bossColour = bossButton.resolvedStyle.backgroundColor;
        endlessColour = endlessButton.resolvedStyle.backgroundColor;
        menuButton.RegisterCallback<ClickEvent>(Menu);
        menuButton.RegisterCallback<NavigationSubmitEvent>(Menu);
        bossButton.RegisterCallback<ClickEvent>(DisplayBossHighscores);
        bossButton.RegisterCallback<NavigationSubmitEvent>(DisplayBossHighscores);
        endlessButton.RegisterCallback<ClickEvent>(DisplayEndlessHighscores);
        endlessButton.RegisterCallback<NavigationSubmitEvent>(DisplayEndlessHighscores);
        //Get the highscores list view
        highscores = document.Q<MultiColumnListView>("MultiColumnListView");
        moreInfoList = document.Q<ListView>("MoreInfoList");
        //Set to be invisible by default
        document.style.display = DisplayStyle.None;
        navigationSetting();
    }

    private void navigationSetting()
    {
        bossButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: menuButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down:
                    highscores.Focus();
                    highscores.selectedIndex = 0;
                    highscores.ScrollToItem(highscores.selectedIndex);
                    break;
                case NavigationMoveEvent.Direction.Left: endlessButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: endlessButton.Focus(); break;
            }
            e.PreventDefault();
        });

        endlessButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: menuButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down:
                    highscores.Focus();
                    highscores.selectedIndex = 0;
                    highscores.ScrollToItem(highscores.selectedIndex);
                    break;
                case NavigationMoveEvent.Direction.Left: bossButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: bossButton.Focus(); break;
            }
            e.PreventDefault();
        });

        menuButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up:
                    highscores.Focus();
                    highscores.selectedIndex = 0;
                    highscores.ScrollToItem(highscores.selectedIndex);
                    break;
                case NavigationMoveEvent.Direction.Down: bossButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: menuButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: menuButton.Focus(); break;
            }
            e.PreventDefault();
        });

        highscores.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: bossButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down: menuButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left:
                case NavigationMoveEvent.Direction.Right:
                    moreInfoList.Focus();
                    moreInfoList.selectedIndex = 0;
                    moreInfoList.ScrollToItem(moreInfoList.selectedIndex);
                    break;
            }
            e.PreventDefault();
        });

        moreInfoList.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: bossButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down: menuButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left:
                case NavigationMoveEvent.Direction.Right:
                    highscores.Focus();
                    highscores.selectedIndex = 0;
                    highscores.ScrollToItem(highscores.selectedIndex);
                    break;
            }
            e.PreventDefault();
        });
    }

    private void Menu(EventBase evt)
    {
        if (document != null)
        {
            if (SceneManager.GetSceneByName("Titlescreen").isLoaded) //If coming from the main menu
            {
                document.style.display = DisplayStyle.None;
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
            else //If coming from in game
            {
                SceneManager.UnloadSceneAsync("Highscores");
            }
        }
    }

    public void DisplayEndlessHighscores(EventBase evt = null) //Nullable so I can call it manually as well
    {
        //Turn endless button grey and boss button to original colour. We need a better and more unified solution for selected buttons
        bossButton.style.backgroundColor = bossColour;
        endlessButton.style.backgroundColor = new StyleColor(new Color(0.5f, 0.5f, 0.5f));
        //Remove endless click event and add boss click event
        endlessButton.UnregisterCallback<ClickEvent>(DisplayEndlessHighscores);
        endlessButton.UnregisterCallback<NavigationSubmitEvent>(DisplayEndlessHighscores);
        bossButton.RegisterCallback<ClickEvent>(DisplayBossHighscores);
        bossButton.RegisterCallback<NavigationSubmitEvent>(DisplayBossHighscores);
        displayedHighscores = GameMode.Endless;
        //Clear selected items
        highscores.ClearSelection();
        clearMoreInfoList();
        DisplayHighscores(Scoreboard.Instance.savedScores.highscores);
    }

    public void DisplayBossHighscores(EventBase evt = null) //Nullable so I can call it manually as well
    {
        //Turn boss button grey and endless button to original colour. We need a better and more unified solution for selected buttons
        bossButton.style.backgroundColor = new StyleColor(new Color(0.5f, 0.5f, 0.5f));
        endlessButton.style.backgroundColor = endlessColour;
        //Remove boss click event and add endless click event
        bossButton.UnregisterCallback<ClickEvent>(DisplayBossHighscores);
        bossButton.UnregisterCallback<NavigationSubmitEvent>(DisplayBossHighscores);
        endlessButton.RegisterCallback<ClickEvent>(DisplayEndlessHighscores);
        endlessButton.RegisterCallback<NavigationSubmitEvent>(DisplayEndlessHighscores);
        displayedHighscores = GameMode.Boss;
        //Clear selected items
        highscores.ClearSelection();
        clearMoreInfoList();
        DisplayHighscores(Scoreboard.Instance.savedScores.highscores);
    }

    //Making MultiColumnListViews are really confusing. This is the result of several hours of trial and error.
    //I used the docs, forum posts, copilot, and chatgpt
    private void DisplayHighscores(List<EntryData> highscoreData)
    {
        //Clear the previous data
        highscores.itemsSource = new List<EntryData>();
        highscores.Rebuild();
        List<EntryData> displayedData = highscoreData.Where(entry => entry.gameMode == displayedHighscores).ToList();

        //If the list is empty return
        if (displayedData == null || displayedData.Count == 0 || displayedData[0] == null)
        {
            return;
        }
        //Set the new data
        highscores.itemsSource = displayedData;

        // Helper method to bind cell data
        void BindCell(VisualElement element, int index, Func<EntryData, string> getValue)
        {
            if (index >= 0 && index < displayedData.Count)
            {
                (element as Label).text = getValue(displayedData[index]);
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
        //Make the list just have an empty string so there is not a "list empty" message
        moreInfoList.itemsSource = new List<string> { "" };
        moreInfoList.Rebuild();
    }
}

