using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField]
    private GameObject HUD;
    [SerializeField]
    private GameObject GameOver;
    public Scoreboard scoreboard;
    private Label pointsText;
    private Label finalscoreText;
    private Label highscoreNotif;
    public ScoreInputField inputField;
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
        //TODO: Check if endless or boss fight
        submitButton.RegisterCallback<ClickEvent>(SubmitEndlessScore);

    }
    
    public void IncreasePoints(int amount)
    {
        enemiesKilled++;
        score += amount;
        pointsText.text = "Points: " + score.ToString();
    }

    public void FinalScore()
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
        scoreboard.AddEntry(playerScoreInfo, true);

        SceneManager.LoadScene("Highscores");

    }

    public void SubmitBossScore(ClickEvent click)
    {
        EntryData playerScoreInfo = new EntryData(
            "Bob", //TODO: Change the input to dynamic
            score,
            WeaponType.Pistol, //Dynamic weapon type when weapon update is merged
            InventoryPage.Instance.GetItems(),
            enemiesKilled);
        scoreboard.AddEntry(playerScoreInfo, false);

        SceneManager.LoadScene("Highscores");
    }
}
