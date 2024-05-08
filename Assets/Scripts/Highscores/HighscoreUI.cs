using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText = null;
    [SerializeField] private TextMeshProUGUI scoreText = null;

    public void Intialise(EntryData entryData)
    {
        nameText.text = entryData.entryName;
        scoreText.text = entryData.entryScore.ToString();
    }
}
