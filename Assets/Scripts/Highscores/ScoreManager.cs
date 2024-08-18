using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField]
    private GameObject HUD;
    [SerializeField]
    private GameObject GameOver;
    private Label pointsText;
    private Label finalscoreText;
    private Label highscoreNotif;
    private Button submitButton;

    public int score = 0;
    public int enemiesKilled = 0;

    void Awake()
    {
        //Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        VisualElement document = HUD.GetComponent<UIDocument>().rootVisualElement;
        VisualElement gameOverDoc = GameOver.GetComponent<UIDocument>().rootVisualElement;

        pointsText = document.Q<Label>("Points");
        finalscoreText = gameOverDoc.Q<Label>("FinalScore");
        highscoreNotif = gameOverDoc.Q<Label>("HighscoreNotif");
        submitButton = gameOverDoc.Q<Button>("SubmitScore");
    }

    public void IncreasePoints(int amount)
    {
        enemiesKilled++;
        score += amount;
        pointsText.text = "Points: " + score.ToString();
    }

    public void FinalScore()
    {
        //REMOVE BRACKETS. Just hiding the commented out code for now. Might need to use some of it?
        {
            //TODO: Fix end game display
            // HighscoreSaveData savedScores = scoreboard.GetSavedScores();
            // if (savedScores == null || savedScores.highscores.Count < 1)
            // {
            //    highscoreNotif.text = "New Highscore!!!";
            //    highscoreNotif.style.color = new StyleColor(new Color32(255, 221, 0, 255)); //Yellow
            // }
            // else
            // {
            //     for (int i = 0; i < savedScores.highscores.Count; i++)
            //     {
            //         //check if score is greater than a saved score
            //         if (score > savedScores.highscores[i].entryScore || savedScores.highscores.Count < scoreboard.maxScoreEntries)
            //         {
            //             highscoreNotif.text = "New Highscore!!!";
            //             highscoreNotif.style.color = new StyleColor(new Color32(255, 221, 0, 255));

            //         }
            //         else
            //         {
            //             highscoreNotif.text = "Skill Issue";
            //             highscoreNotif.style.color = new StyleColor(new Color32(255, 0, 5, 255)); //Red
            //         }
            //     }
            // }
        }

        if (GameSettings.waveNumber == 25 && PlayerStats.Instance.CurrentHealth > 0) //If wave 25 then submit a boss score. Otherwise submit an endless score. Need to check if the player is alive to know if they won the fight
        {
            submitButton.RegisterCallback<ClickEvent>(SubmitBossScore);
            //TODO: Need to let the player keep playing if they want in Endless mode
        }
        else
        {
            submitButton.RegisterCallback<ClickEvent>(SubmitEndlessScore);
        }
        finalscoreText.text = "Score: " + score.ToString();
    }

    public void SubmitEndlessScore(ClickEvent click)
    {
        EntryData playerScoreInfo = new EntryData(
            "Bob", //TODO: Change the input to dynamic
            score,
            WeaponType.Pistol, //Dynamic weapon type when weapon update is merged
            GameSettings.waveNumber,
            InventoryPage.Instance.GetItems(),
            enemiesKilled);
        StartCoroutine(SubmitHighScore(playerScoreInfo, SubmitEndlessScore, true));
    }

    public void SubmitBossScore(ClickEvent click)
    {
        EntryData playerScoreInfo = new EntryData(
            "Bob", //TODO: Change the input to dynamic
            score,
            WeaponType.Pistol, //Dynamic weapon type when weapon update is merged
            InventoryPage.Instance.GetItems(),
            enemiesKilled);
        StartCoroutine(SubmitHighScore(playerScoreInfo, SubmitBossScore, false));

    }


    IEnumerator SubmitHighScore(EntryData entry, EventCallback<ClickEvent> click, bool isEndless) //Click needs to be passed in because it can be an endless or boss click event
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("HighScores", LoadSceneMode.Additive);
        submitButton.text = "Submitting...";
        submitButton.UnregisterCallback<ClickEvent>(click); //So the player can't submit multiple times
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        submitButton.text = "Submitted!"; //Will only be seen when highscores unloaded
        //Following code is modified from the Menu.cs script. It is used to get the UI document of the scene loaded
        Scene loadedScene = SceneManager.GetSceneByName("HighScores"); //Getting the scene I just loaded
        if (loadedScene.IsValid())
        {
            GameObject[] rootObjects = loadedSce
            ne.GetRootGameObjects(); // Gets an array of all the objects in the scene that aren't inside other objects
            UIDocument uiDocument = rootObjects
                .Select(obj => obj.GetComponent<UIDocument>())
                .FirstOrDefault(doc => doc != null); // Checking each object to see if it has a UIDocument component, and if it does, it returns it
            if (uiDocument != null)
            {
                VisualElement rootElement = uiDocument.rootVisualElement; // Getting the root visual element of the UI document
                rootElement.style.display = DisplayStyle.Flex;
            }
        }
        Scoreboard scoreboard = FindObjectOfType<Scoreboard>();
        scoreboard.AddEntry(entry, isEndless);
    }
}
