using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text pointsText;
    public TMP_Text highscoreText;

    public int score;
    public int highscore;
    // Start is called before the first frame update
    void Start()
    {
        //EnemyHealth.OnEnemyDeath.AddListener(IncreasePoints);
        pointsText.text = "Score: " + score.ToString();
        highscoreText.text = "Highscore: " + highscore.ToString();
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

    private void Awake()
    {
        Instance = this;
    }

}
