using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public TMP_Text pointsText;
    public TMP_Text highscoreText;

    int points = 0;
    int highscore = 0;
    // Start is called before the first frame update
    void Start()
    {
        pointsText.text = "Points: " + points.ToString();
        highscoreText.text = "Highscore: " + highscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
