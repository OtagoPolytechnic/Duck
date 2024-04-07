using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager scoreManager;
    public TMP_Text pointsText;
    public TMP_Text highscoreText;

    public int points = 0;
    public int highscore = 0;
    // Start is called before the first frame update
    void Start()
    {
        //EnemyHealth.enemy.OnEnemyDeath.AddListener(IncreasePoints());
        pointsText.text = "Points: " + points.ToString();
        highscoreText.text = "Highscore: " + highscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Awake()
    {
        scoreManager = this;
    }

    private void IncreasePoints(int amount)
    {
        points += amount;
        pointsText.text = "Points: " + points.ToString();
    }

}
