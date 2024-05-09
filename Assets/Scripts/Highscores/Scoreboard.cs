using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private int maxScoreEntries;

    [SerializeField] private Transform scoreContainerTransform;
    [SerializeField] private GameObject highscoreEntry;

    [Header ("Test")]
    [SerializeField] EntryData testEntryData = new EntryData();

    // adds path based on user system
    // example path for windows: Users\hartlr3\AppData\LocalLow\DefaultCompany\DuckGame
    private string savePath => $"{Application.persistentDataPath}/highscores.json";

    private void Start()
    {
        HighscoreSaveData savedScores = GetSavedScores();

        UpdateUI(savedScores);
        SaveScores(savedScores);
    }

    [ContextMenu("Add Test Entry")]
    public void AddTestEntry()
    {
        AddEntry(testEntryData);
    }
    public void AddEntry(EntryData entryData)
    {
        HighscoreSaveData savedScores = GetSavedScores();

        bool scoreAdded = false;

        for(int i = 0; i < savedScores.highscores.Count; i++) 
        {
            //check if score is greater than a saved score
            if(entryData.entryScore > savedScores.highscores[i].entryScore)
            {
                savedScores.highscores.Insert(i, entryData);
                scoreAdded = true;
                break;
            }
        }

        //check if space to add entry
        if(!scoreAdded && savedScores.highscores.Count < maxScoreEntries)
        {
            savedScores.highscores.Add(entryData);
        }

        if (savedScores.highscores.Count > maxScoreEntries)
        {
            savedScores.highscores.RemoveRange(
                maxScoreEntries, 
                savedScores.highscores.Count - maxScoreEntries);
        }

        UpdateUI(savedScores);
        SaveScores(savedScores);
    }
    private HighscoreSaveData GetSavedScores()
    {
        if(!File.Exists(savePath))
        {
            File.Create(savePath).Dispose();
            return new HighscoreSaveData();
        }

        using (StreamReader stream = new StreamReader(savePath))
        {
            string json = stream.ReadToEnd();
            
            return JsonUtility.FromJson<HighscoreSaveData>(json);
        }
    }

    private void SaveScores(HighscoreSaveData highscoreSaveData)
    {
        using(StreamWriter stream = new StreamWriter(savePath))
        {
            //true displays the json in a nice format
            string json = JsonUtility.ToJson(highscoreSaveData, true);

            stream.Write(json);
        }
    }

    private void UpdateUI(HighscoreSaveData savedScores)
    {
        foreach (Transform child in scoreContainerTransform)
        {
            Destroy(child.gameObject);
        }

        foreach(EntryData highscore in savedScores.highscores)
        {
            Instantiate(highscoreEntry, scoreContainerTransform).
                GetComponent<HighscoreUI>().Intialise(highscore);
        }
    }
}
