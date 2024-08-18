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
    private TextField playerName;
    private Label enterName;
    private Button replay;
    private VisualElement gameOverDoc;
    private Label GameOverText;

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
        gameOverDoc = GameOver.GetComponent<UIDocument>().rootVisualElement;

        pointsText = document.Q<Label>("Points");
        finalscoreText = gameOverDoc.Q<Label>("FinalScore");
        highscoreNotif = gameOverDoc.Q<Label>("HighscoreNotif");
        submitButton = gameOverDoc.Q<Button>("SubmitScore");
        playerName = gameOverDoc.Q<TextField>("PlayerName");
        enterName = gameOverDoc.Q<Label>("EnterName");
        replay = gameOverDoc.Q<Button>("Replay");
        GameOverText = gameOverDoc.Q<Label>("Title");
        
    }

    public void IncreasePoints(int amount)
    {
        enemiesKilled++;
        score += amount;
        pointsText.text = "Points: " + score.ToString();
    }

    public IEnumerator FinalScore()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("HighScores", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        submitButton.style.display = DisplayStyle.Flex;
        playerName.style.display = DisplayStyle.Flex;
        enterName.style.display = DisplayStyle.Flex;
        gameOverDoc.style.display = DisplayStyle.Flex;
        if (GameSettings.waveNumber == 25 && PlayerStats.Instance.CurrentHealth > 0) //If wave 25 then submit a boss score. Otherwise submit an endless score. Need to check if the player is alive to know if they won the fight
        {
            if (Scoreboard.Instance.CheckTopScore(score, false)) //If the player got the top score
            {
                highscoreNotif.text = "TOP BOSS SCORE!";
            }
            else if (Scoreboard.Instance.CheckHighScore(score, false)) //If the player got a high score
            {
                highscoreNotif.text = "New boss high score!";
            }
            else
            {
                highscoreNotif.text = "Continue?";
                //Hide the submit button and text field
                submitButton.style.display = DisplayStyle.None;
                playerName.style.display = DisplayStyle.None;
                enterName.style.display = DisplayStyle.None;
            }
            submitButton.RegisterCallback<ClickEvent>(SubmitBossScore);
            replay.UnregisterCallback<ClickEvent>(GameManager.Instance.Restart);
            replay.text = "Continue";
            GameOverText.text = "BOSS KILLED!";
            replay.RegisterCallback<ClickEvent>(Continue);
        }
        else
        {
            if (Scoreboard.Instance.CheckTopScore(score, false)) //If the player got the top score
            {
                highscoreNotif.text = "TOP ENDLESS SCORE!";
            }
            else if (Scoreboard.Instance.CheckHighScore(score, false)) //If the player got a high score
            {
                highscoreNotif.text = "New endless high score!";
            }
            else
            {
                highscoreNotif.text = "Game over";
                //Hide the submit button and text field
                submitButton.style.display = DisplayStyle.None;
                playerName.style.display = DisplayStyle.None;
                enterName.style.display = DisplayStyle.None;
            }
            submitButton.RegisterCallback<ClickEvent>(SubmitEndlessScore);
        }
        finalscoreText.text = "Score: " + score.ToString();
    }

    public void SubmitEndlessScore(ClickEvent click)
    {
        if (playerName.value == "") //If the player didn't enter a name then default to "Player"
        {
            playerName.value = "Player";
        }
        EntryData playerScoreInfo = new EntryData(
            playerName.value,
            score,
            WeaponType.Pistol, //Dynamic weapon type when weapon update is merged
            GameSettings.waveNumber,
            InventoryPage.Instance.GetItems(),
            enemiesKilled);
        SubmitHighScore(playerScoreInfo, SubmitEndlessScore, true);
    }

    public void SubmitBossScore(ClickEvent click)
    {
        if (playerName.value == "") //If the player didn't enter a name then default to "Player"
        {
            playerName.value = "Player";
        }
        EntryData playerScoreInfo = new EntryData(
            playerName.value,
            score,
            WeaponType.Pistol, //Dynamic weapon type when weapon update is merged
            InventoryPage.Instance.GetItems(),
            enemiesKilled);
        SubmitHighScore(playerScoreInfo, SubmitBossScore, false);

    }


    public void SubmitHighScore(EntryData entry, EventCallback<ClickEvent> click, bool isEndless) //Click needs to be passed in because it can be an endless or boss click event
    {
        submitButton.UnregisterCallback<ClickEvent>(click); //So the player can't submit multiple times
        submitButton.text = "Submitted!"; //Will only be seen when highscores unloaded
        //Following code is modified from the Menu.cs script. It is used to get the UI document of the scene loaded
        Scene loadedScene = SceneManager.GetSceneByName("HighScores"); //Getting the scene I just loaded
        if (loadedScene.IsValid())
        {
            GameObject[] rootObjects = loadedScene.GetRootGameObjects(); // Gets an array of all the objects in the scene that aren't inside other objects
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

    public void Continue(ClickEvent click)
    {
        StartCoroutine(continueGame());
    }

    IEnumerator continueGame()
    {
        submitButton.text = "Submit";
        replay.UnregisterCallback<ClickEvent>(Continue);
        replay.RegisterCallback<ClickEvent>(GameManager.Instance.Restart);
        GameOverText.text = "GAME OVER";
        gameOverDoc.style.display = DisplayStyle.None;
        //It has an error if I don't do this even though the scene is loaded
        if (SceneManager.GetSceneByName("HighScores").isLoaded)
        {
            SceneManager.UnloadSceneAsync("HighScores");
        }
        yield return new WaitUntil(() => !SceneManager.GetSceneByName("HighScores").isLoaded);
        //Resetting everything that might have changed
        //Unload the highscores scene
        Timer.Instance.EndWave();
    }
}
