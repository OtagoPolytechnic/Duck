using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using System.Linq;
using UnityEngine.InputSystem;

public class ScoreManager : MonoBehaviour
{
    public InputActionAsset inputActions;
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
        pointsText.text = "POINTS: " + score.ToString();
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

        if (Scoreboard.Instance.CheckTopScore(score, GameSettings.gameMode)) //If the player got the top score
        {
            highscoreNotif.text = "TOP SCORE!";
        }
        else if (Scoreboard.Instance.CheckHighScore(score, GameSettings.gameMode)) //If the player got a high score
        {
            highscoreNotif.text = "New high score!";
        }
        else
        {
            highscoreNotif.text = "Game over";
            //Hide the submit button and text field
            submitButton.style.display = DisplayStyle.None;
            playerName.style.display = DisplayStyle.None;
            enterName.style.display = DisplayStyle.None;
        }
        submitButton.RegisterCallback<ClickEvent>(SubmitScore);
        submitButton.RegisterCallback<KeyDownEvent>(SubmitScore);
        finalscoreText.text = "Score: " + score.ToString();
        if (GameSettings.gameState == GameState.BossVictory)
        {
            GameOverText.text = "BOSS KILLED!";
        }
    }

    public void SubmitScore(EventBase evt)
    {
        if (SubmitCheck.Submit(evt, inputActions))
        {
            if (playerName.value == "") //If the player didn't enter a name then default to "Player"
            {
                playerName.value = "Player";
            }
            EntryData playerScoreInfo = new EntryData(
                playerName.value,
                score,
                WeaponStats.Instance.CurrentWeapon,
                GameSettings.waveNumber,
                ItemPanel.Instance.HighscoreItems(),
                enemiesKilled,
                GameSettings.gameState == GameState.BossVictory, //Returns true if the final boss was killed
                GameSettings.gameMode,
                GameSettings.activeSkill);
            
            submitButton.text = "Submitted!";
            submitButton.UnregisterCallback<ClickEvent>(SubmitScore); //So the player can't submit multiple times
            submitButton.UnregisterCallback<KeyDownEvent>(SubmitScore);

            Scene loadedScene = SceneManager.GetSceneByName("HighScores");
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
                Scoreboard scoreboard = FindObjectOfType<Scoreboard>();
                scoreboard.AddEntry(playerScoreInfo);
                if (GameSettings.gameMode == GameMode.Boss) 
                {
                    HighscoreUI.Instance.DisplayBossHighscores();
                }
                else
                {
                    HighscoreUI.Instance.DisplayEndlessHighscores();
                }
            }
        }
    }
}
