using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HighscoreUI : MonoBehaviour
{
    private VisualElement document;
    private ListView highscores;
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
        highscores = document.Q<ListView>("Highscores");
    }

    private void Menu(ClickEvent click)
    {
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
    }

    public void DisplayHighscores(List<EntryData> highscoreData)
    {
        //Bind the data to the list view
        highscores.itemsSource = highscoreData;
        highscores.makeItem = () => new Label();
        highscores.bindItem = (e, i) => (e as Label).text = $"{highscoreData[i].entryName} - {highscoreData[i].entryScore} - {highscoreData[i].weapon}";
        highscores.fixedItemHeight = 100;
        highscores.Rebuild();
    }
}   

