using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public Scoreboard scoreboard;
    public TMP_Text pointsText;
    public TMP_Text finalscoreText;
    public TMP_Text highscoreNotif;
     
    public ScoreInputField inputField;
    public GameObject submitButton;

    public EntryData playerScoreInfo = new EntryData();

    public int score;
    
    // Start is called before the first frame update
    void Start()
    {
        //couldn't get this working now, but might save for later
        //EnemyHealth.OnEnemyDeath.AddListener(IncreasePoints);

        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void Awake()
    {
        Instance = this;
    }

    public void SumbitPlayerScore()
    {
        playerScoreInfo.entryName = inputField.playerName;
        playerScoreInfo.entryScore = score;
        scoreboard.AddEntry(playerScoreInfo);

        SceneManager.LoadScene("Highscores");
    }
}
