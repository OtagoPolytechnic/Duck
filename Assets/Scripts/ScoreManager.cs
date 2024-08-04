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
    public Label finalscoreText;
    public Label highscoreNotif;
    public ScoreInputField inputField;
    public GameObject submitButton;

    public EntryData playerScoreInfo = new EntryData();

    public int score = 0;

    void Awake()
    {
        Instance = this;
        VisualElement document = HUD.GetComponent<UIDocument>().rootVisualElement;
        VisualElement gameOverDoc = GameOver.GetComponent<UIDocument>().rootVisualElement;
        
        pointsText = document.Q<Label>("Points");
        finalscoreText = gameOverDoc.Q<Label>("FinalScore");

    }
    
    public void IncreasePoints(int amount)
    {
        score += amount;
        pointsText.text = "Points: " + score.ToString();
    }

    public void FinalScore()
    {
        //Highscore notif code removed because of bug when there was no intial highscores file on local
        
        // HighscoreSaveData savedScores = scoreboard.GetSavedScores();

        // for (int i = 0; i < savedScores.highscores.Count; i++)
        // {
        //     //check if score is greater than a saved score
        //     if (finalscore > savedScores.highscores[i].entryScore || savedScores.highscores.Count < scoreboard.maxScoreEntries)
        //     {
        //         highscoreNotif.text = "<color=#03fcdb>New Highscore!!!</color>";

        //         //display submit button
        //         submitButton.SetActive(true);
        //     }
        //     else
        //     {
        //         highscoreNotif.text = "<color=red>Skill Issue</color>";
        //     }
        // }

        finalscoreText.text = "Score: " + score.ToString();
    }

    public void SumbitPlayerScore()
    {
        playerScoreInfo.entryName = inputField.playerName;
        playerScoreInfo.entryScore = score;
        scoreboard.AddEntry(playerScoreInfo);

        SceneManager.LoadScene("Highscores");
    }
}
