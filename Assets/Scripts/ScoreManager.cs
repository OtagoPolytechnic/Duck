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

    public EntryData playerScoreInfo = new EntryData();

    public int score = 0;

    void Awake()
    {
        Instance = this;
        VisualElement document = HUD.GetComponent<UIDocument>().rootVisualElement;
        VisualElement gameOverDoc = GameOver.GetComponent<UIDocument>().rootVisualElement;
        
        pointsText = document.Q<Label>("Points");
        finalscoreText = gameOverDoc.Q<Label>("FinalScore");
        highscoreNotif = gameOverDoc.Q<Label>("HighscoreNotif");
        submitButton = gameOverDoc.Q<Button>("SubmitScore");
        submitButton.RegisterCallback<ClickEvent>(SubmitPlayerScore);

    }
    
    public void IncreasePoints(int amount)
    {
        score += amount;
        pointsText.text = "Points: " + score.ToString();
    }

    public void FinalScore()
    {

        
        HighscoreSaveData savedScores = scoreboard.GetSavedScores();
        if (savedScores == null || savedScores.highscores.Count < 1)
        {
           highscoreNotif.text = "New Highscore!!!";
           highscoreNotif.style.color = new StyleColor(new Color32(255, 221, 0, 255)); //Yellow
        }
        else
        {
            for (int i = 0; i < savedScores.highscores.Count; i++)
            {
                //check if score is greater than a saved score
                if (score > savedScores.highscores[i].entryScore || savedScores.highscores.Count < scoreboard.maxScoreEntries)
                {
                    highscoreNotif.text = "New Highscore!!!";
                    highscoreNotif.style.color = new StyleColor(new Color32(255, 221, 0, 255));

                }
                else
                {
                    highscoreNotif.text = "Skill Issue";
                    highscoreNotif.style.color = new StyleColor(new Color32(255, 0, 5, 255)); //Red
                }
            }
        }


        finalscoreText.text = "Score: " + score.ToString();
    }

    public void SubmitPlayerScore(ClickEvent click)
    {
        playerScoreInfo.entryName = inputField.playerName;
        playerScoreInfo.entryScore = score;
        scoreboard.AddEntry(playerScoreInfo);

        SceneManager.LoadScene("Highscores");
    }
}
